using System.Collections.Generic;
using System.Threading.Tasks;
using NetCodeTT.Lobby;
using UnityEngine;

namespace View.Core
{
    public class GameLobbyManager : MonoBehaviour
    {
        private LobbyManager _lobbyManager;
        
        public void Init (LobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;
        }

        public async Task<bool> CreateLobby()
        {
            Dictionary<string, string> playerData = new Dictionary<string, string>
            {
                {"GamerTag", "HostPlayer"}
            };
            bool succeeded = await _lobbyManager.CreateLobby(playerData);
            return succeeded;
        }

        public async Task<bool> JoinLobby(string lobbyCode)
        {
            Dictionary<string, string> playerData = new Dictionary<string, string>
            {
                {"GamerTag", "JoinPlayer"}
            };
            bool succeeded = await _lobbyManager.JoinLobby(lobbyCode, playerData);
            return succeeded;
        }
    }
}