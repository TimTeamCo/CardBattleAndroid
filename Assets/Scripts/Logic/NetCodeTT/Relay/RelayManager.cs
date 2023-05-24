using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Http;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using NetworkEvent = Unity.Networking.Transport.NetworkEvent;

namespace NetCodeTT.Relay
{
    public class RelayManager : IRelay
    {
        public NetworkDriver HostDriver;
        public NetworkDriver PlayerDriver;
        public string JoinCode;

        private NetworkConnection clientConnection;
        private bool isRelayServerConnected = false;
        //private UnityTransport unityTransport;
        private string _joinCode;
        private string _ip;
        private int _port;

        private byte[] _connectionData;

        private Guid _allocationId;

        public async Task<string> CreateRelay()
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);
            _joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
            _ip = dtlsEndpoint.Host;
            _port = dtlsEndpoint.Port;
            _allocationId = allocation.AllocationId;
            _connectionData = allocation.ConnectionData;

            return _joinCode;
        }

        public async Task<bool> JoinRelay(string joinCode)
        {
            _joinCode = joinCode;
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            
            RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
            _ip = dtlsEndpoint.Host;
            _port = dtlsEndpoint.Port;
            _allocationId = allocation.AllocationId;
            _connectionData = allocation.ConnectionData;

            return true;
        }

        public string GetAllocationId()
        {
            return _allocationId.ToString();
        }

        public string GetConnectionData()
        {
            return _connectionData.ToString();
        }

        // Set utility functions for constructing server data objects
        private static RelayAllocationId ConvertFromAllocationIdBytes(byte[] allocationIdBytes)
        {
            unsafe
            {
                fixed (byte* ptr = allocationIdBytes)
                {
                    return RelayAllocationId.FromBytePointer(ptr, allocationIdBytes.Length);
                }
            }
        }

        private static RelayConnectionData ConvertConnectionData(byte[] connectionData)
        {
            unsafe
            {
                fixed (byte* ptr = connectionData)
                {
                    return RelayConnectionData.FromBytePointer(ptr, RelayConnectionData.k_Length);
                }
            }
        }

        private static RelayHMACKey ConvertFromHMAC(byte[] hmac)
        {
            unsafe
            {
                fixed (byte* ptr = hmac)
                {
                    return RelayHMACKey.FromBytePointer(ptr, RelayHMACKey.k_Length);
                }
            }
        }

        private static RelayServerEndpoint GetEndpointForConnectionType(List<RelayServerEndpoint> endpoints,
            string connectionType)
        {
            foreach (var endpoint in endpoints)
            {
                if (endpoint.ConnectionType == connectionType)
                {
                    return endpoint;
                }
            }

            return null;
        }

        public static RelayServerData HostRelayData(Allocation allocation, string connectionType = "dtls")
        {
            // Select endpoint based on desired connectionType
            var endpoint = GetEndpointForConnectionType(allocation.ServerEndpoints, connectionType);
            if (endpoint == null)
            {
                throw new Exception($"endpoint for connectionType {connectionType} not found");
            }

            // Prepare the server endpoint using the Relay server IP and port
            var serverEndpoint = NetworkEndPoint.Parse(endpoint.Host, (ushort) endpoint.Port);

            // UTP uses pointers instead of managed arrays for performance reasons, so we use these helper functions to convert them
            var allocationIdBytes = ConvertFromAllocationIdBytes(allocation.AllocationIdBytes);
            var connectionData = ConvertConnectionData(allocation.ConnectionData);
            var key = ConvertFromHMAC(allocation.Key);

            // Prepare the Relay server data and compute the nonce value
            // The host passes its connectionData twice into this function
            var relayServerData = new RelayServerData(ref serverEndpoint, 0, ref allocationIdBytes, ref connectionData,
                ref connectionData, ref key, connectionType == "dtls");

            return relayServerData;
        }

        public static RelayServerData PlayerRelayData(JoinAllocation allocation, string connectionType = "dtls")
        {
            // Select endpoint based on desired connectionType
            var endpoint = GetEndpointForConnectionType(allocation.ServerEndpoints, connectionType);
            if (endpoint == null)
            {
                throw new Exception($"endpoint for connectionType {connectionType} not found");
            }

            // Prepare the server endpoint using the Relay server IP and port
            var serverEndpoint = NetworkEndPoint.Parse(endpoint.Host, (ushort) endpoint.Port);

            // UTP uses pointers instead of managed arrays for performance reasons, so we use these helper functions to convert them
            var allocationIdBytes = ConvertFromAllocationIdBytes(allocation.AllocationIdBytes);
            var connectionData = ConvertConnectionData(allocation.ConnectionData);
            var hostConnectionData = ConvertConnectionData(allocation.HostConnectionData);
            var key = ConvertFromHMAC(allocation.Key);

            // Prepare the Relay server data and compute the nonce values
            // A player joining the host passes its own connectionData as well as the host's
            var relayServerData = new RelayServerData(ref serverEndpoint, 0, ref allocationIdBytes, ref connectionData,
                ref hostConnectionData, ref key, connectionType == "dtls");

            return relayServerData;
        }

        public IEnumerator Example_StartingNetworkDriverAsHost() 
        {
            // Request list of valid regions
            var regionsTask = RelayService.Instance.ListRegionsAsync();

            while (regionsTask.IsCompleted == false)
            {
                yield return null;
            }

            if (regionsTask.IsFaulted)
            {
                Debug.LogError("List regions request failed");
                yield break;
            }

            var regionList = regionsTask.Result;
            // pick a region from the list
            var targetRegion = regionList[0].Id;

            // Request an allocation to the Relay service
            // with a maximum of 2 peer connections, for a maximum of 6 players.
            var relayMaxConnections = 2;
            var allocationTask = RelayService.Instance.CreateAllocationAsync(relayMaxConnections, targetRegion);

            while (allocationTask.IsCompleted == false)
            {
                yield return null;
            }

            if (allocationTask.IsFaulted)
            {
                Debug.LogError("Create allocation request failed");
                yield break;
            }

            var allocation = allocationTask.Result;

            // Request the join code to the Relay service
            var joinCodeTask = RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            while (joinCodeTask.IsCompleted == false)
            {
                yield return null;
            }

            if (joinCodeTask.IsFaulted)
            {
                Debug.LogError("Create join code request failed");
                yield break;
            }

            // Get the Join Code, you can then share it with the clients so they can join
            JoinCode = joinCodeTask.Result;

            // Format the server data, based on desired connectionType
            var relayServerData = HostRelayData(allocation, "dtls");

            // Bind and listen to the Relay server
            yield return Example_BindAndListenAsHostPlayer(relayServerData);
        }

        private IEnumerator Example_BindAndListenAsHostPlayer(RelayServerData relayServerData)
        {
            // Create the NetworkDriver using the Relay server data
            var settings = new NetworkSettings();
            settings.WithRelayParameters(serverData: ref relayServerData);
            HostDriver = NetworkDriver.Create(settings);

            // Bind the NetworkDriver to the local endpoint
            if (HostDriver.Bind(NetworkEndPoint.AnyIpv4) != 0)
            {
                Debug.LogError("Server failed to bind");
            }
            else
            {
                // The binding process is an async operation; wait until bound
                while (HostDriver.Bound == false)
                {
                    HostDriver.ScheduleUpdate().Complete();
                    yield return null;
                }

                // Once the driver is bound you can start listening for connection requests
                if (HostDriver.Listen() != 0)
                {
                    Debug.LogError("Server failed to listen");
                }
                else
                {
                    isRelayServerConnected = true;
                }
            }
        }

        public IEnumerator Example_StartNetworkDriverAsConnectingPlayer(string relayJoinCode) 
        {
            // Send the join request to the Relay service
            var joinTask = RelayService.Instance.JoinAllocationAsync(relayJoinCode);

            while (joinTask.IsCompleted == false) yield return null;

            if (joinTask.IsFaulted)
            {
                Debug.LogError("Join Relay request failed");
                yield break;
            }

            // Collect and convert the Relay data from the join response
            var allocation = joinTask.Result;

            // Format the server data, based on desired connectionType
            var relayServerData = PlayerRelayData(allocation, "dtls");

            yield return Example_BindAndConnectToHost(relayServerData);
        }

        private IEnumerator Example_BindAndConnectToHost(RelayServerData relayServerData)
        {
            // Create the NetworkDriver using the Relay server data
            var settings = new NetworkSettings();
            settings.WithRelayParameters(serverData: ref relayServerData);
            PlayerDriver = NetworkDriver.Create(settings);

            // Bind the NetworkDriver to the available local endpoint.
            // This will send the bind request to the Relay server
            if (PlayerDriver.Bind(NetworkEndPoint.AnyIpv4) != 0)
            {
                Debug.LogError("Client failed to bind");
            }
            else
            {
                while (PlayerDriver.Bound == false)
                {
                    PlayerDriver.ScheduleUpdate().Complete();
                    yield return null;
                }

                // Once the client is bound to the Relay server, you can send a connection request
                clientConnection = PlayerDriver.Connect(relayServerData.Endpoint);
            }
        }

        async void CreateGame()
        {
            Allocation a = await RelayService.Instance.CreateAllocationAsync(2);
            JoinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        }


        async void JoinGame()
        {
            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(JoinCode);

        }
    }
}