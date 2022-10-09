using System;
using System.Collections;
using TTLobbyLogic;
using TTNetCode;
using Unity.Networking.Transport;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace TTRelay
{
    // Host logic: Request a new Allocation, and then both bind to it and request a join code. Once those are both complete, supply data back to the lobby.
    public class RelayUtpSetupHost : RelayUtpSetup
    {
        [Flags]
        private enum JoinState
        {
            None = 0,
            Bound = 1,
            Joined = 2
        }

        private JoinState m_joinState = JoinState.None;
        private Allocation m_allocation;

        protected override void JoinRelay()
        {
            RelayAPIInterface.AllocateAsync(m_localLobby.MaxPlayerCount, OnAllocation);
        }

        private void OnAllocation(Allocation allocation)
        {
            m_allocation = allocation;
            RelayAPIInterface.GetJoinCodeAsync(allocation.AllocationId, OnRelayCode);
            bool isSecure = false;
            m_endpointForServer = GetEndpointForAllocation(allocation.ServerEndpoints, allocation.RelayServer.IpV4,
                allocation.RelayServer.Port, out isSecure);
            BindToAllocation(m_endpointForServer, allocation.AllocationIdBytes, allocation.ConnectionData,
                allocation.ConnectionData, allocation.Key, 16, isSecure);
        }

        private void OnRelayCode(string relayCode)
        {
            m_localLobby.RelayCode = relayCode;
            m_localLobby.RelayServer =
                new ServerAddress(AddressFromEndpoint(m_endpointForServer), m_endpointForServer.Port);
            m_joinState |= JoinState.Joined;
            CheckForComplete();
        }

        protected override void OnBindingComplete()
        {
            if (m_networkDriver.Listen() != 0)
            {
                Debug.LogError("RelayUtpSetupHost failed to bind to the Relay Allocation.");
                m_onJoinComplete(false, null);
            }
            else
            {
                Debug.Log("Relay host is bound.");
                m_joinState |= JoinState.Bound;
                CheckForComplete();
            }
        }

        private void CheckForComplete()
        {
            if (m_joinState == (JoinState.Joined | JoinState.Bound) &&
                this != null) // this will equal null (i.e. this component has been destroyed) if the host left the lobby during the Relay connection sequence.
            {
                m_isRelayConnected = true;
                RelayUtpHost host = gameObject.AddComponent<RelayUtpHost>();
                host.Initialize(m_networkDriver, m_connections, m_localUser, m_localLobby);
                m_onJoinComplete(true, host);
                LobbyAsyncRequests.Instance.UpdatePlayerRelayInfoAsync(m_allocation.AllocationId.ToString(),
                    m_localLobby.RelayCode, null);
            }
        }
    }

    /// <summary>
    /// Client logic: Wait until the Relay join code is retrieved from the lobby's shared data. Then, use that code to get the Allocation to bind to, and
    /// then create a connection to the host.
    /// </summary>
    public class RelayUtpSetupClient : RelayUtpSetup
    {
        private JoinAllocation m_allocation;

        protected override void JoinRelay()
        {
            m_localLobby.onChanged += OnLobbyChange;
        }

        private void OnLobbyChange(LocalLobby lobby)
        {
            if (m_localLobby.RelayCode != null)
            {
                RelayAPIInterface.JoinAsync(m_localLobby.RelayCode, OnJoin);
                m_localLobby.onChanged -= OnLobbyChange;
            }
        }

        private void OnJoin(JoinAllocation joinAllocation)
        {
            if (joinAllocation == null || this == null) // The returned JoinAllocation is null if allocation failed. this would be destroyed already if you quit the lobby while Relay is connecting.
            {
                return;
            }
            m_allocation = joinAllocation;
            bool isSecure = false;
            m_endpointForServer = GetEndpointForAllocation(joinAllocation.ServerEndpoints,
                joinAllocation.RelayServer.IpV4, joinAllocation.RelayServer.Port, out isSecure);
            BindToAllocation(m_endpointForServer, joinAllocation.AllocationIdBytes, joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData, joinAllocation.Key, 1, isSecure);
            m_localLobby.RelayServer = new ServerAddress(AddressFromEndpoint(m_endpointForServer), m_endpointForServer.Port);
        }

        protected override void OnBindingComplete()
        {
            StartCoroutine(ConnectToServer());
        }

        private IEnumerator ConnectToServer()
        {
            // Once the client is bound to the Relay server, send a connection request.
            m_connections.Add(m_networkDriver.Connect(m_endpointForServer));
            while (m_networkDriver.GetConnectionState(m_connections[0]) == NetworkConnection.State.Connecting)
            {
                m_networkDriver.ScheduleUpdate().Complete();
                yield return null;
            }

            if (m_networkDriver.GetConnectionState(m_connections[0]) != NetworkConnection.State.Connected)
            {
                Debug.LogError("RelayUtpSetupClient could not connect to the host.");
                m_onJoinComplete(false, null);
            }
            else if (this != null)
            {
                m_isRelayConnected = true;
                RelayUtpClient client = gameObject.AddComponent<RelayUtpClient>();
                client.Initialize(m_networkDriver, m_connections, m_localUser, m_localLobby);
                m_onJoinComplete(true, client);
                LobbyAsyncRequests.Instance.UpdatePlayerRelayInfoAsync(m_allocation.AllocationId.ToString(),
                    m_localLobby.RelayCode, null);
            }
        }
    }
}