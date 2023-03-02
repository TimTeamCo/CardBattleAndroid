using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace LobbyRelaySample.ngo
{
    public class RoundChecker : NetworkBehaviour
    {
        [SerializeField] NetworkedDataStore _dataStore;
        [SerializeField] TextMeshProUGUI _roundOutputText;
        [SerializeField] UnityEvent<PlayerData> _onRoundEnd;
        private ulong _localId;

        // Called on the host.
        public void NewRound(ulong id)
        {
            int newRound = _dataStore.UpdateRound(id, 1);
            UpdateRoundOutput_ClientRpc(id, newRound);
        }
        
        [ClientRpc]
        void UpdateRoundOutput_ClientRpc(ulong id, int round)
        {
            if (_localId == id)
                _roundOutputText.text = round.ToString("00");
        }
        
        public void OnRoundEnd()
        {
            _dataStore.GetAllPlayerData(_onRoundEnd);
        }
    }
}