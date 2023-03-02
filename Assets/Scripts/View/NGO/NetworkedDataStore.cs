using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine.Events;

namespace LobbyRelaySample.ngo
{
    // A place to store data needed by networked behaviors. Each client has an instance so they can retrieve data, but the server's instance stores the actual data.
    public class NetworkedDataStore : NetworkBehaviour
    {
        public static NetworkedDataStore Instance;
        private Dictionary<ulong, PlayerData> m_playerData = new Dictionary<ulong, PlayerData>();
        private ulong _localId;
        
        Action<PlayerData> _onGetCurrentCallback;
        UnityEvent<PlayerData> _onEachPlayerCallback;

        public void Awake()
        {
            Instance = this;
        }
        
        public override void OnNetworkSpawn()
        {
            _localId = NetworkManager.Singleton.LocalClientId;
        }
        
        public void AddPlayer(ulong id, string name)
        {
            if (!IsServer)
                return;

            if (!m_playerData.ContainsKey(id))
                m_playerData.Add(id, new PlayerData(name, id, 0, 0));
            else
                m_playerData[id] = new PlayerData(name, id, 0, 0);
        }
        
        /// <returns>The updated round for the player matching the id after adding the delta, or int.MinValue otherwise.</returns>
        public int UpdateRound(ulong id, int delta)
        {
            if (!IsServer)
                return int.MinValue;

            if (!m_playerData.ContainsKey(id)) 
                return int.MinValue;
            
            m_playerData[id].round += delta;
            return m_playerData[id].round;
        }
        
        public int UpdateScore(ulong id, int delta)
        {
            if (!IsServer)
                return int.MinValue;

            if (!m_playerData.ContainsKey(id)) 
                return int.MinValue;
            
            m_playerData[id].score += delta;
            return m_playerData[id].score;
        }
        
        // Retrieve the data for all players in order from 1st to last place, calling onEachPlayer for each.
        public void GetAllPlayerData(UnityEvent<PlayerData> onEachPlayer)
        {
            _onEachPlayerCallback = onEachPlayer;
            GetAllPlayerData_ServerRpc(_localId);
        }
        
        [ServerRpc(RequireOwnership = false)]
        void GetAllPlayerData_ServerRpc(ulong callerId)
        {
            //TODO Now win when score more - rewrite order by count of squads units
            var sortedData = m_playerData.Select(kvp => kvp.Value).OrderByDescending(data => data.score);
            GetAllPlayerData_ClientRpc(callerId, sortedData.ToArray());
        }
        
        [ClientRpc]
        void GetAllPlayerData_ClientRpc(ulong callerId, PlayerData[] sortedData)
        {
            if (callerId != _localId)
                return;

            int rank = 1;
            foreach (var data in sortedData)
            {
                _onEachPlayerCallback.Invoke(data);
                rank++;
            }
            _onEachPlayerCallback = null;
        }
        
        // Retreive the data for one player, passing it to the onGet callback.
        public void GetPlayerData(ulong targetId, Action<PlayerData> onGet)
        {
            _onGetCurrentCallback = onGet;
            GetPlayerData_ServerRpc(targetId, _localId);
        }
        
        [ServerRpc(RequireOwnership = false)]
        void GetPlayerData_ServerRpc(ulong id, ulong callerId)
        {
            if (m_playerData.ContainsKey(id))
                GetPlayerData_ClientRpc(callerId, m_playerData[id]);
            else
                GetPlayerData_ClientRpc(callerId, new PlayerData(null, 0));
        }

        [ClientRpc]
        public void GetPlayerData_ClientRpc(ulong callerId, PlayerData data)
        {
            if (callerId == _localId)
            {   _onGetCurrentCallback?.Invoke(data);
                _onGetCurrentCallback = null;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (Instance == this)
                Instance = null;
        }
    }
}