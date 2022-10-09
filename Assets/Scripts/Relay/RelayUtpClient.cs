using System;
using System.Collections.Generic;
using TTGame;
using TTLobbyLogic;
using Unity.Networking.Transport;
using UnityEngine;

namespace TTRelay
{
    /// This observes the local player and updates remote players over Relay when there are local changes, demonstrating basic data transfer over the Unity Transport (UTP).
    /// Created after the connection to Relay has been confirmed.
    /// If you are using the Unity Networking Package, you can use their Relay instead of building your own packets.
    public class RelayUtpClient : MonoBehaviour, IDisposable
    {
        protected LobbyUser m_localUser;
        protected LocalLobby m_localLobby;
        protected NetworkDriver m_networkDriver;
        protected List<NetworkConnection> m_connections; // For clients, this has just one member, but for hosts it will have more.
        protected bool m_IsRelayConnected { get { return m_localLobby.RelayServer != null; } }

        protected bool m_hasSentInitialMessage = false;
        private const float k_heartbeatPeriod = 5;
        private bool m_hasDisposed = false;

        protected enum UtpMessageType
        {
            Ping = 0,
            NewPlayer,
            PlayerApprovalState,
            ReadyState,
            PlayerName,
            Emote,
            StartCountdown,
            CancelCountdown,
            ConfirmInGame,
            EndInGame,
            PlayerDisconnect
        }
        
        public virtual void Initialize(NetworkDriver networkDriver, List<NetworkConnection> connections, LobbyUser localUser, LocalLobby localLobby)
        {
            m_localUser = localUser;
            m_localLobby = localLobby;
            m_localUser.onChanged += OnLocalChange;
            m_networkDriver = networkDriver;
            m_connections = connections;
            Locator.Get.UpdateSlow.Subscribe(UpdateSlow, k_heartbeatPeriod);
        }
        
        private void Update()
        {
            OnUpdate();
        }
        
        /// Clients need to send any data over UTP periodically, or else Relay will remove them from the allocation.
        private void UpdateSlow(float dt)
        {
            // If disconnected from Relay for some reason, we *want* this client to timeout.
            if (m_IsRelayConnected == false)
            {
                return;
            }
            
            foreach (NetworkConnection connection in m_connections)
            {
                // The ID doesn't matter here, so send a minimal number of bytes.
                WriteByte(m_networkDriver, connection, "0", UtpMessageType.Ping, 0); 
            }
        }
        
        protected virtual void OnUpdate()
        {
            if (m_hasSentInitialMessage == false)
            {
                // Just on the first execution, make sure to handle any events that accumulated while completing the connection.
                ReceiveNetworkEvents(m_networkDriver);
            }
            
            // This pumps all messages, which pings the Relay allocation and keeps it alive. It should be called no more often than ReceiveNetworkEvents.
            m_networkDriver.ScheduleUpdate().Complete();
            
            // This reads the message queue which was just updated.
            ReceiveNetworkEvents(m_networkDriver);
            if (m_hasSentInitialMessage == false)
            {
                // On a client, the 0th (and only) connection is to the host.
                SendInitialMessage(m_networkDriver, m_connections[0]); 
            }
        }

        private void ReceiveNetworkEvents(NetworkDriver driver)
        {
            NetworkConnection conn;
            DataStreamReader strm;
            NetworkEvent.Type cmd; 
            // NetworkConnection also has PopEvent, but NetworkDriver.PopEvent automatically includes new connections.
            while ((cmd = driver.PopEvent(out conn, out strm)) != NetworkEvent.Type.Empty)
            {
                ProcessNetworkEvent(conn, strm, cmd);
            }
        }
        
        // See the Write* methods for the expected event format.
        private void ProcessNetworkEvent(NetworkConnection connection, DataStreamReader dataStreamReader, NetworkEvent.Type commandType)
        {
            if (commandType == NetworkEvent.Type.Data)
            {
                List<byte> msgContents = new List<byte>(ReadMessageContents(ref dataStreamReader));
                // We require at a minimum - Message type, the length of the user ID, and the user ID.
                if (msgContents.Count < 3)
                {
                    return;
                }

                UtpMessageType msgType = (UtpMessageType)msgContents[0];
                int idLength = msgContents[1];
                if (msgContents.Count < idLength + 2)
                {   
                    Debug.LogWarning($"Relay client processed message of length {idLength}, but contents were of length {msgContents.Count}.");
                    return;
                }

                string id = System.Text.Encoding.UTF8.GetString(msgContents.GetRange(2, idLength).ToArray());
                if (CanProcessDataEventFor(connection, msgType, id) == false)
                {
                    return;
                }
                
                msgContents.RemoveRange(0, 2 + idLength);

                switch (msgType)
                {
                    case UtpMessageType.PlayerApprovalState:
                    {
                        Approval approval = (Approval) msgContents[0];
                        if (approval == Approval.OK && (m_localUser.IsApproved) == false)
                        {
                            OnApproved(m_networkDriver, connection);
                        }
                        else if (approval == Approval.GameAlreadyStarted)
                        {
                            Locator.Get.Messenger.OnReceiveMessage(MessageType.DisplayErrorPopup, "Rejected: Game has already started.");
                        }
                        break;
                    }
                    case UtpMessageType.PlayerName:
                    {
                        int nameLength = msgContents[0];
                        string name = System.Text.Encoding.UTF8.GetString(msgContents.GetRange(1, nameLength).ToArray());
                        m_localLobby.LobbyUsers[id].DisplayName = name;
                        break;
                    }
                    case UtpMessageType.Emote:
                    {
                        EmoteType emote = (EmoteType)msgContents[0];
                        m_localLobby.LobbyUsers[id].Emote = emote;
                        break;
                    }
                    case UtpMessageType.ReadyState:
                    {
                        UserStatus status = (UserStatus)msgContents[0];
                        m_localLobby.LobbyUsers[id].UserStatus = status;
                        break;
                    }
                    case UtpMessageType.StartCountdown:
                    {
                        Locator.Get.Messenger.OnReceiveMessage(MessageType.StartCountdown, null);
                        break;
                    }
                    case UtpMessageType.CancelCountdown:
                    {
                        Locator.Get.Messenger.OnReceiveMessage(MessageType.CancelCountdown, null);
                        break;
                    }
                    case UtpMessageType.ConfirmInGame:
                    {
                        Locator.Get.Messenger.OnReceiveMessage(MessageType.ConfirmInGameState, null);
                        break;
                    }
                    case UtpMessageType.EndInGame:
                    {
                        Locator.Get.Messenger.OnReceiveMessage(MessageType.EndGame, null);
                        break;
                    }
                }

                ProcessNetworkEventDataAdditional(connection, msgType, id);
            }
            else if (commandType == NetworkEvent.Type.Disconnect)
            {
                ProcessDisconnectEvent(connection, dataStreamReader);
            }
        }
        
        protected virtual bool CanProcessDataEventFor(NetworkConnection conn, UtpMessageType type, string id)
        {
            // Don't react to our own messages. Also, don't need to hold onto messages if the ID is absent; clients should be initialized and in the lobby before they send events.
            // (Note that this enforces lobby membership before processing any events besides an approval request, so a client is unable to fully use Relay unless they're in the lobby.)
            return id != m_localUser.ID && (m_localUser.IsApproved && m_localLobby.LobbyUsers.ContainsKey(id) || type == UtpMessageType.PlayerApprovalState);
        }
        
        protected virtual void ProcessNetworkEventDataAdditional(NetworkConnection conn, UtpMessageType msgType, string id) { }
        
        protected virtual void ProcessDisconnectEvent(NetworkConnection conn, DataStreamReader strm)
        {
            // The host disconnected, and Relay does not support host migration. So, all clients should disconnect.
            string msg;
            if (m_IsRelayConnected)
            {
                msg = "The host disconnected! Leaving the lobby.";
            }
            else
            {
                msg = "Connection to host was lost. Leaving the lobby.";
            }

            Debug.LogError(msg);
            
            Locator.Get.Messenger.OnReceiveMessage(MessageType.DisplayErrorPopup, msg);
            Leave();
            Locator.Get.Messenger.OnReceiveMessage(MessageType.ChangeMenuState, GameState.Menu);
        }
        
        /// UTP uses raw pointers for efficiency (i.e. C-style byte* instead of byte[]).
        /// ReadMessageContents converts them back to byte arrays, assuming the stream contains 1 byte for array length followed by contents.
        /// Any actual pointer manipulation and so forth happens service-side, so we simply need to convert back to a byte array here.
        unsafe private byte[] ReadMessageContents(ref DataStreamReader strm) // unsafe is required to access the pointer.
        {
            int length = strm.Length;
            byte[] bytes = new byte[length];
            fixed (byte * ptr = bytes)
            {
                strm.ReadBytes(ptr, length);
            }
            return bytes;
        }
        
        // Once a client is connected, send a message out alerting the host.
        private void SendInitialMessage(NetworkDriver driver, NetworkConnection connection)
        {
            WriteByte(driver, connection, m_localUser.ID, UtpMessageType.NewPlayer, 0);
            m_hasSentInitialMessage = true;
        }
        
        private void OnApproved(NetworkDriver driver, NetworkConnection connection)
        {
            m_localUser.IsApproved = true;
            ForceFullUserUpdate(driver, connection, m_localUser);
        }
        
        // When player data is updated, send out events for just the data that actually changed.
        private void DoUserUpdate(NetworkDriver driver, NetworkConnection connection, LobbyUser user)
        {
            // Only update with actual changes. (If multiple change at once, we send messages for each separately, but that shouldn't happen often.)
            if (0 < (user.LastChanged & LobbyUser.UserMembers.DisplayName))
            {
                WriteString(driver, connection, user.ID, UtpMessageType.PlayerName, user.DisplayName);
            }

            if (0 < (user.LastChanged & LobbyUser.UserMembers.Emote))
            {
                WriteByte(driver, connection, user.ID, UtpMessageType.Emote, (byte)user.Emote);
            }

            if (0 < (user.LastChanged & LobbyUser.UserMembers.UserStatus))
            {
                WriteByte(driver, connection, user.ID, UtpMessageType.ReadyState, (byte)user.UserStatus);
            }
        }

        // Sometimes (e.g. when a new player joins), we need to send out the full current state of this player.
        protected void ForceFullUserUpdate(NetworkDriver driver, NetworkConnection connection, LobbyUser user)
        {
            // Note that it would be better to send a single message with the full state, but for the sake of shorter code we'll leave that out here.
            WriteString(driver, connection, user.ID, UtpMessageType.PlayerName, user.DisplayName);
            WriteByte(driver, connection, user.ID, UtpMessageType.Emote, (byte)user.Emote);
            WriteByte(driver, connection, user.ID, UtpMessageType.ReadyState, (byte)user.UserStatus);
        }
        
        // Write string data as: [1 byte: msgType] [1 byte: id length N] [N bytes: id] [1 byte: string length M] [M bytes: string]
        protected void WriteString(NetworkDriver driver, NetworkConnection connection, string id, UtpMessageType msgType, string str)
        {
            byte[] idBytes = System.Text.Encoding.UTF8.GetBytes(id);
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(str);

            List<byte> message = new List<byte>(idBytes.Length + strBytes.Length + 3); // Extra 3 bytes for the msgType plus the ID and message lengths.
            message.Add((byte)msgType);
            message.Add((byte)idBytes.Length);
            message.AddRange(idBytes);
            message.Add((byte)strBytes.Length);
            message.AddRange(strBytes);
            SendMessageData(driver, connection, message);
        }
        
        /// Write byte data as: [1 byte: msgType] [1 byte: id length N] [N bytes: id] [1 byte: data]
        protected void WriteByte(NetworkDriver driver, NetworkConnection connection, string id, UtpMessageType msgType, byte value)
        {
            byte[] idBytes = System.Text.Encoding.UTF8.GetBytes(id);
            List<byte> message = new List<byte>(idBytes.Length + 3); // Extra 3 bytes for the msgType, ID length, and the byte value.
            message.Add((byte)msgType);
            message.Add((byte)idBytes.Length);
            message.AddRange(idBytes);
            message.Add(value);
            SendMessageData(driver, connection, message);
        }
        
        private void SendMessageData(NetworkDriver driver, NetworkConnection connection, List<byte> message)
        {
            if (driver.BeginSend(connection, out var dataStream) == 0)
            {
                byte[] bytes = message.ToArray();
                unsafe // Similarly to ReadMessageContents, our data must be converted to a pointer before being sent.
                {
                    fixed (byte * bytesPtr = bytes)
                    {
                        dataStream.WriteBytes(bytesPtr, message.Count);
                        driver.EndSend(dataStream);
                    }
                }
            }
        }

        private void OnLocalChange(LobbyUser localUser)
        {
            if (m_connections.Count == 0) // This could be the case for the host alone in the lobby.
            {
                return;
            }

            foreach (NetworkConnection conn in m_connections)
            {
                DoUserUpdate(m_networkDriver, conn, m_localUser);
            }
        }
        
        public void Dispose()
        {
            if (m_hasDisposed)
            {
                return;
            }
            
            UnInitialize();
            m_hasDisposed = true;
        }
        
        protected virtual void UnInitialize()
        {
            m_localUser.onChanged -= OnLocalChange;
            Leave();
            Locator.Get.UpdateSlow.Unsubscribe(UpdateSlow);
            // Don't clean up the NetworkDriver here, or else our disconnect message won't get through to the host.
            // The host will handle cleaning up the connection.
        }
        
        /// Disconnect from Relay, usually while leaving the lobby. (You can also call this elsewhere to see how Lobby will detect a Relay disconnect automatically.)
        public virtual void Leave()
        {
            foreach (NetworkConnection connection in m_connections)
            {
                // If the client calls Disconnect, the host might not become aware right away (depending on when the PubSub messages get pumped), so send a message over UTP instead.
                WriteByte(m_networkDriver, connection, m_localUser.ID, UtpMessageType.PlayerDisconnect, 0);
            }

            m_localLobby.RelayServer = null;
        }

        public void OnDestroy()
        {
            Dispose();
        }
    }
}