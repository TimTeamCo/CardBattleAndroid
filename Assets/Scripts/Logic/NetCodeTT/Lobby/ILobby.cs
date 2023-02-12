using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Game;

namespace NetCodeTT.Lobby
{
    using Unity.Services.Lobbies.Models;
    
    public interface ILobby
    {
        void CreateLobby(Action<string> result);
        
        void JoinLobbyByID(string lobbyID);
        
        void JoinLobbyByLobbyCode(string lobbyCode);
        
        void QuickJoin(Action<string> result);
        
        void DeleteAllCreatedLobbies();
        
        void DeleteLobby(string lobbyId);
        
        Task<List<string>> GetJoinedLobbies();
        
        void UpdateLobbyData();
        
        void UpdatePlayerData();
        
        void FilteringLobbySample();
        
        Task Reconnect(string lobbyId);

        void LeaveLobby();

        Task GetLobby(string lobbyId, Action<Lobby> lobbyRes);

        Task<Lobby> CreateLobbyAsync(int maxPlayers, bool isPrivate, LocalPlayer localUser);
    }
}