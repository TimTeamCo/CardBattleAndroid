using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace PlayerData
{
    public class LobbyPlayerData
    {
        public string Id => _id;
        public string GamerTag => _gamerTag;
        public string Nickname => _nickname;

        public bool IsReady
        {
            get => _isReady;
            set => _isReady = value;
        }
        
        private string _id;
        private string _gamerTag;
        private string _nickname;
        private bool _isReady;

        public void Init(string id, string gamerTag, string nickname)
        {
            _id = id;
            _gamerTag = gamerTag;
            _nickname = nickname;
        }

        public void Init(Dictionary<string, PlayerDataObject> playerData)
        {
            UpdateState(playerData);
        }

        public void UpdateState(Dictionary<string, PlayerDataObject> playerData)
        {
            if (playerData.ContainsKey("Id"))
            {
                _id = playerData["Id"].Value;
            }

            if (playerData.ContainsKey("GamerTag"))
            {
                _gamerTag = playerData["GamerTag"].Value;
            }
            
            if (playerData.ContainsKey("Nickname"))
            {
                _nickname = playerData["Nickname"].Value;
            }
            
            if (playerData.ContainsKey("IsReady"))
            {
                _isReady = playerData["IsReady"].Value == "True";
            }
        }

        public Dictionary<string, string> Serialize()
        {
            return new Dictionary<string, string>
            {
                {"Id", _id},
                {"GamerTag", _gamerTag},
                {"Nickname", _nickname},
                {"IsReady", _isReady.ToString()}
            };
        }
    }
}