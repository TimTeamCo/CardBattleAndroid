using System;
using System.Collections;
using System.Collections.Generic;
using TTGame;
using TTLobbyLogic;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace TTRelay
{
    /// Responsible for setting up a connection with Relay using Unity Transport (UTP). A Relay Allocation is created by the host, and then all players
    /// bind UTP to that Allocation in order to send data to each other.
    /// Must be a MonoBehaviour since the binding process doesn't have asynchronous callback options.
    public abstract class RelayUtpSetup : MonoBehaviour
    {
        protected bool m_isRelayConnected = false;
        protected NetworkDriver m_networkDriver;
        protected List<NetworkConnection> m_connections;
        protected NetworkEndPoint m_endpointForServer;
        protected LocalLobby m_localLobby;
        protected LobbyUser m_localUser;
        protected Action<bool, RelayUtpClient> m_onJoinComplete;

        public static string AddressFromEndpoint(NetworkEndPoint endpoint)
        {
            return endpoint.Address.Split(':')[0];
        }

        public void BeginRelayJoin(LocalLobby localLobby, LobbyUser localUser,
            Action<bool, RelayUtpClient> onJoinComplete)
        {
            m_localLobby = localLobby;
            m_localUser = localUser;
            m_onJoinComplete = onJoinComplete;
            JoinRelay();
        }

        protected abstract void JoinRelay();

        // Determine the server endpoint for connecting to the Relay server, for either an Allocation or a JoinAllocation.
        // If DTLS encryption is available, and there's a secure server endpoint available, use that as a secure connection.
        // Otherwise, just connect to the Relay IP unsecured.
        public static NetworkEndPoint GetEndpointForAllocation(List<RelayServerEndpoint> endpoints, string ip, int port,
            out bool isSecure)
        {
#if ENABLE_MANAGED_UNITYTLS
            foreach (RelayServerEndpoint endpoint in endpoints)
            {
                if (endpoint.Secure && endpoint.Network == RelayServerEndpoint.NetworkOptions.Udp)
                {
                    isSecure = true;
                    return NetworkEndPoint.Parse(endpoint.Host, (ushort) endpoint.Port);
                }
            }
#endif
            isSecure = false;
            return NetworkEndPoint.Parse(ip, (ushort) port);
        }

        // Shared behavior for binding to the Relay allocation, which is required for use.
        // Note that a host will send bytes from the Allocation it creates, whereas a client will send bytes from the JoinAllocation it receives using a relay code.
        protected void BindToAllocation(NetworkEndPoint serverEndpoint, byte[] allocationIdBytes,
            byte[] connectionDataBytes, byte[] hostConnectionDataBytes, byte[] hmacKeyBytes, int connectionCapacity,
            bool isSecure)
        {
            RelayAllocationId allocationId = ConvertAllocationIdBytes(allocationIdBytes);
            RelayConnectionData connectionData = ConvertConnectionDataBytes(connectionDataBytes);
            RelayConnectionData hostConnectionData = ConvertConnectionDataBytes(hostConnectionDataBytes);
            RelayHMACKey key = ConvertHMACKeyBytes(hmacKeyBytes);

            var relayServerData = new RelayServerData(ref serverEndpoint, 0, ref allocationId, ref connectionData,
                ref hostConnectionData, ref key, isSecure);
            
            // For security, the nonce value sent when authenticating the allocation must be increased.
            relayServerData.ComputeNewNonce();
            var networkSettings = new NetworkSettings();

            m_networkDriver = NetworkDriver.Create(networkSettings.WithRelayParameters(ref relayServerData));
            m_connections = new List<NetworkConnection>(connectionCapacity);

            if (m_networkDriver.Bind(NetworkEndPoint.AnyIpv4) != 0)
            {
                Debug.LogError("Failed to bind to Relay allocation.");
            }
            else
            {
                StartCoroutine(WaitForBindComplete());
            }
        }
        
        private IEnumerator WaitForBindComplete()
        {
            while (m_networkDriver.Bound == false)
            {
                m_networkDriver.ScheduleUpdate().Complete();
                yield return null;
            }
            OnBindingComplete();
        }
        
        protected abstract void OnBindingComplete();
        
        #region UTP uses pointers instead of managed arrays for performance reasons, so we use these helper functions to convert them.
        unsafe private static RelayAllocationId ConvertAllocationIdBytes(byte[] allocationIdBytes)
        {
            fixed (byte* ptr = allocationIdBytes)
            {
                return RelayAllocationId.FromBytePointer(ptr, allocationIdBytes.Length);
            }
        }

        unsafe private static RelayConnectionData ConvertConnectionDataBytes(byte[] connectionData)
        {
            fixed (byte* ptr = connectionData)
            {
                return RelayConnectionData.FromBytePointer(ptr, RelayConnectionData.k_Length);
            }
        }

        unsafe private static RelayHMACKey ConvertHMACKeyBytes(byte[] hmac)
        {
            fixed (byte* ptr = hmac)
            {
                return RelayHMACKey.FromBytePointer(ptr, RelayHMACKey.k_Length);
            }
        }
        #endregion
        
        private void OnDestroy()
        {
            if (m_isRelayConnected == false && m_networkDriver.IsCreated)
            {
                m_networkDriver.Dispose();
            }
        }
    }
}