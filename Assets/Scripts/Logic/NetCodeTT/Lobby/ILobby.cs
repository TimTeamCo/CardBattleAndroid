using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCodeTT.Lobby
{
    using Unity.Services.Lobbies.Models;
    
    public interface ILobby
    {
        Task<Lobby> QuickJoin(LocalPlayer localUser);
        
        Task<Lobby> CreateLobby(LocalPlayer localUser);
        
        Task LeaveLobbyAsync();
        
        void JoinLobbyByID(string lobbyID);
        
        void JoinLobbyByLobbyCode(string lobbyCode);
        
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