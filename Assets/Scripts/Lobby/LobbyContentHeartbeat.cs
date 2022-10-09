using System;
using System.Collections.Generic;
using TTGame;
using TTRelay;
using Unity.Services.Lobbies.Models;

namespace TTLobbyLogic
{
    /// Keep updated on changes to a joined lobby, at a speed compliant with Lobby's rate limiting.
    public class LobbyContentHeartbeat : IReceiveMessages
    {
        private LocalLobby m_localLobby;
        private LobbyUser m_localUser;
        private int m_awaitingQueryCount = 0;
        private bool m_shouldPushData = false;

        // Used for determining if a user should timeout if they are unable to connect.
        private const float k_approvalMaxTime = 10;
        private float m_lifetime = 0;

        public void BeginTracking(LocalLobby lobby, LobbyUser localUser)
        {
            m_localLobby = lobby;
            m_localUser = localUser;
            Locator.Get.UpdateSlow.Subscribe(OnUpdate, 1.5f);
            Locator.Get.Messenger.Subscribe(this);
            m_localLobby.onChanged += OnLocalLobbyChanged;
            // Ensure the initial presence of a new player is pushed to the lobby; otherwise, when a non-host joins, the LocalLobby never receives their data until they push something new.
            m_shouldPushData = true;
            m_lifetime = 0;
        }

        public void EndTracking()
        {
            m_shouldPushData = false;
            Locator.Get.UpdateSlow.Unsubscribe(OnUpdate);
            Locator.Get.Messenger.Unsubscribe(this);
            if (m_localLobby != null)
            {
                m_localLobby.onChanged -= OnLocalLobbyChanged;
            }
            m_localLobby = null;
            m_localUser = null;
        }

        private void OnLocalLobbyChanged(LocalLobby changed)
        {
            // When the player leaves, their LocalLobby is cleared out but maintained.
            if (string.IsNullOrEmpty(changed.LobbyID))
            {
                EndTracking();
            }
            m_shouldPushData = true;
        }

        public void OnReceiveMessage(MessageType type, object msg)
        {
            if (type == MessageType.ClientUserSeekingDisapproval)
            {
                // By not refreshing, it's possible to have a lobby in the lobby list UI after its countdown starts and then try joining.
                bool shouldDisapprove = m_localLobby.State != LobbyState.Lobby;
                if (shouldDisapprove)
                {
                    (msg as Action<Approval>)?.Invoke(Approval.GameAlreadyStarted);
                }
            }
        }

        // If there have been any data changes since the last update, push them to Lobby. Regardless, pull for the most recent data.
        // (Unless we're already awaiting a query, in which case continue waiting.)
        private void OnUpdate(float dt)
        {
            m_lifetime += dt;
            if (m_awaitingQueryCount > 0 || m_localLobby == null)
                return;
            if (m_localUser.IsHost)
                LobbyAsyncRequests.Instance.DoLobbyHeartbeat(dt);

            if (!m_localUser.IsApproved && m_lifetime > k_approvalMaxTime)
            {
                Locator.Get.Messenger.OnReceiveMessage(MessageType.DisplayErrorPopup, "Connection attempt timed out!");
                Locator.Get.Messenger.OnReceiveMessage(MessageType.ChangeMenuState, GameState.Menu);
            }

            if (m_shouldPushData)
            {
                PushDataToLobby();
            }
            else
            {
                OnRetrieve();
            }


            void PushDataToLobby()
            {
                m_shouldPushData = false;

                if (m_localUser.IsHost)
                {
                    m_awaitingQueryCount++;
                    DoLobbyDataPush();
                }
                m_awaitingQueryCount++;
                DoPlayerDataPush();
            }

            void DoLobbyDataPush()
            {
                LobbyAsyncRequests.Instance.UpdateLobbyDataAsync(RetrieveLobbyData(m_localLobby), () => { if (--m_awaitingQueryCount <= 0) OnRetrieve(); });
            }

            void DoPlayerDataPush()
            {
                LobbyAsyncRequests.Instance.UpdatePlayerDataAsync(RetrieveUserData(m_localUser), () => { if (--m_awaitingQueryCount <= 0) OnRetrieve(); });
            }

            void OnRetrieve()
            {
                Lobby lobbyRemote = LobbyAsyncRequests.Instance.CurrentLobby;
                if (lobbyRemote == null) return;
                bool prevShouldPush = m_shouldPushData;
                var prevState = m_localLobby.State;
                ToLocalLobby.Convert(lobbyRemote, m_localLobby);
                m_shouldPushData = prevShouldPush;

                // If the host suddenly leaves, the Lobby service will automatically handle disconnects after about 30s, but we can try to do a disconnect sooner if we detect it.
                if (!m_localUser.IsHost)
                {
                    foreach (var lobbyUser in m_localLobby.LobbyUsers)
                    {
                        if (lobbyUser.Value.IsHost)
                        {
                            return;
                        }
                    }
                    Locator.Get.Messenger.OnReceiveMessage(MessageType.DisplayErrorPopup, "Host left the lobby! Disconnecting...");
                    Locator.Get.Messenger.OnReceiveMessage(MessageType.EndGame, null);
                    Locator.Get.Messenger.OnReceiveMessage(MessageType.ChangeMenuState, GameState.Menu);
                }
            }
        }

        private static Dictionary<string, string> RetrieveLobbyData(LocalLobby lobby)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("RelayCode", lobby.RelayCode);
            data.Add("RelayNGOCode", lobby.RelayNGOCode);
            // Using an int is smaller than using the enum state's name.
            data.Add("State", ((int)lobby.State).ToString());
            data.Add("Color", ((int)lobby.Color).ToString());
            data.Add("State_LastEdit", lobby.Data.State_LastEdit.ToString());
            data.Add("Color_LastEdit", lobby.Data.Color_LastEdit.ToString());
            data.Add("RelayNGOCode_LastEdit", lobby.Data.RelayNGOCode_LastEdit.ToString());
            return data;
        }

        private static Dictionary<string, string> RetrieveUserData(LobbyUser user)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            if (user == null || string.IsNullOrEmpty(user.ID))
                return data;
            // The lobby doesn't need to know any data beyond the name and state; Relay will handle the rest.
            data.Add("DisplayName", user.DisplayName);
            data.Add("UserStatus", ((int)user.UserStatus).ToString());
            return data;
        }
    }
}