using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCodeTT.Lobbys;
using NetCodeTT.Relay;
using PlayerData;
using Saver;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace View.Core
{
    public class GameLobbyManager : MonoBehaviour
    {
        private LobbyManager _lobbyManager;
        private RelayManager _relayManager;
        private List<LobbyPlayerData> _lobbyPlayerDatas = new List<LobbyPlayerData>();
        private LobbyPlayerData _localUserPlayerData;
        private LobbyData _lobbyData;
        
        public bool IsHost => _lobbyManager.IsHostUser();

        public void Init (LobbyManager lobbyManager, RelayManager relayManager)
        {
            _lobbyManager = lobbyManager;
            _relayManager = relayManager;
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

            int numberOfReadyPlayers = 0;
            foreach (var data in playersData)
            {
                LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
                lobbyPlayerData.Init(data);

                if (lobbyPlayerData.IsReady)
                {
                    numberOfReadyPlayers++;
                }
                
                if (lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
                {
                    _localUserPlayerData = lobbyPlayerData;
                }

                _lobbyPlayerDatas.Add(lobbyPlayerData);
            }

            _lobbyData = new LobbyData();
            _lobbyData.Init(lobby.Data);
            
            LobbyEvents.OnLobbyUpdated?.Invoke();
            
            if (numberOfReadyPlayers == lobby.MaxPlayers)
            {
                LobbyEvents.OnLobbyReady?.Invoke();
            }
        }

        public async Task<bool> CreateLobby()
        {
            _lobbyData = new LobbyData();
            
            LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
            lobbyPlayerData.Init(AuthenticationService.Instance.PlayerId, "HostPlayer", LocalSaver.GetPlayerNickname(), Enum.Parse<PetType>(LocalSaver.GetPlayerPet()));
            bool succeeded = await _lobbyManager.CreateLobby(lobbyPlayerData.Serialize(), _lobbyData.Serialize());
            return succeeded;
        }

        public async Task<bool> JoinLobby(string lobbyCode)
        {
            LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
            lobbyPlayerData.Init(AuthenticationService.Instance.PlayerId, "JoinPlayer", LocalSaver.GetPlayerNickname(), Enum.Parse<PetType>(LocalSaver.GetPlayerPet()));
            bool succeeded = await _lobbyManager.JoinLobby(lobbyCode, lobbyPlayerData.Serialize());
            return succeeded;
        }

        public List<LobbyPlayerData> GetPlayers()
        {
            return _lobbyPlayerDatas;
        }

        public async Task<bool> SetPlayerReady()
        {
            _localUserPlayerData.IsReady = true;
            return await _lobbyManager.UpdatePlayerData(_localUserPlayerData.Id, _localUserPlayerData.Serialize());
        }
        
        public async Task SetNewPet(PetType pet)
        {
            _localUserPlayerData.Pet = pet;
            await _lobbyManager.UpdatePlayerData(_localUserPlayerData.Id, _localUserPlayerData.Serialize());
        }

        public async Task StartGame()
        {
            if (IsHost)
            {
                _lobbyData.JoinRelayCode = await _relayManager.CreateRelay();
                string allocationID = _relayManager.GetAllocationId();
                string conectionData = _relayManager.GetConnectionData();
                await _lobbyManager.UpdatePlayerData(_localUserPlayerData.Id, _localUserPlayerData.Serialize(), allocationID, conectionData);
            }
            SceneManager.LoadScene("Battle");
        }
    }
}