using System.Collections.Generic;
using System.Threading.Tasks;
using NetCodeTT.Lobbys;
using PlayerData;
using Saver;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace View.Core
{
    public class GameLobbyManager : MonoBehaviour
    {
        private LobbyManager _lobbyManager;
        private List<LobbyPlayerData> _lobbyPlayerDatas = new List<LobbyPlayerData>();
        private LobbyPlayerData _localUserPlayerData;

        public void Init (LobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;
        }

        private void OnEnable()
        {
            NetCodeTT.Lobbys.LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            NetCodeTT.Lobbys.LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }


        private void OnLobbyUpdated(Lobby lobby)
        {
            List<Dictionary<string, PlayerDataObject>> playersData = _lobbyManager.GetPlayersData();
            _lobbyPlayerDatas.Clear();

            foreach (var data in playersData)
            {
                LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
                lobbyPlayerData.Init(data);

                if (lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
                {
                    _localUserPlayerData = lobbyPlayerData;
                }

                _lobbyPlayerDatas.Add(lobbyPlayerData);
            }
            
            LobbyEvents.OnLobbyUpdated?.Invoke();
        }

        public async Task<bool> CreateLobby()
        {
            LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
            lobbyPlayerData.Init(AuthenticationService.Instance.PlayerId, "HostPlayer", LocalSaver.GetPlayerNickname());
            bool succeeded = await _lobbyManager.CreateLobby(lobbyPlayerData.Serialize());
            return succeeded;
        }

        public async Task<bool> JoinLobby(string lobbyCode)
        {
            LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
            lobbyPlayerData.Init(AuthenticationService.Instance.PlayerId, "JoinPlayer", LocalSaver.GetPlayerNickname());
            bool succeeded = await _lobbyManager.JoinLobby(lobbyCode, lobbyPlayerData.Serialize());
            return succeeded;
        }

        public List<LobbyPlayerData> GetPlayers()
        {
            return _lobbyPlayerDatas;
        }
    }
}