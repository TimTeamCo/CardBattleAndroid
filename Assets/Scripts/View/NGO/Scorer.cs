using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace LobbyRelaySample.ngo
{
    public class Scorer : NetworkBehaviour
    {
        [SerializeField] NetworkedDataStore _dataStore;
        [SerializeField] TextMeshProUGUI _scoreOutputText;
        
        [Tooltip("When the game ends, this will be called once for each player in order of rank (1st-place first, and so on).")]
        [SerializeField] UnityEvent<PlayerData> _onGameEnd;
        
        private ulong _localId;

        public override void OnNetworkSpawn()
        {
            _localId = NetworkManager.Singleton.LocalClientId;
        }

        // Called on the host.
        public void ScoreSuccess(ulong id)
        {
            int newScore = _dataStore.UpdateScore(id, 1);
            UpdateScoreOutput_ClientRpc(id, newScore);
        }
        
        public void ScoreFailure(ulong id)
        {
            int newScore = _dataStore.UpdateScore(id, -1);
            UpdateScoreOutput_ClientRpc(id, newScore);
        }

        [ClientRpc]
        void UpdateScoreOutput_ClientRpc(ulong id, int score)
        {
            if (_localId == id)
                _scoreOutputText.text = score.ToString("00");
        }

        public void OnGameEnd()
        {
            _dataStore.GetAllPlayerData(_onGameEnd);
        }
    }
}