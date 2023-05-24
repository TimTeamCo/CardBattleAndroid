using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace NetCodeTT.Lobbys
{
    public class LobbyData
    {
        public string JoinRelayCode
        {
            get => _joinRelayCode;
            set => _joinRelayCode = value;
        }
        
        private string _joinRelayCode;

        public void Init(Dictionary<string, DataObject> lobbyData)
        {
            UpdateState(lobbyData);
        }

        private void UpdateState(Dictionary<string, DataObject> lobbyData)
        {
            if (lobbyData.ContainsKey("JoinRelayCode"))
            {
                _joinRelayCode = lobbyData["JoinRelayCode"].Value;
            }
        }

        public Dictionary<string, string> Serialize()
        {
            return new Dictionary<string, string>
            {
                {"JoinRelayCode", _joinRelayCode}
            };
        }
    }
}