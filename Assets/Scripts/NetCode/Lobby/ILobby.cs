using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCode.Lobby
{
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

        Task GetLobby(string lobbyId, Action<string> result);
    }
}