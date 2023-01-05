using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCode.Lobby
{
    public interface ILobby
    {
        void CreateLobby();

        void JoinLobbyByID(string lobbyID);
        
        void JoinLobbyByLobbyCode(string lobbyCode);
        
        void DeleteAllCreatedLobbies();
        
        Task<List<string>> GetJoinedLobbies();
        
        void UpdateLobbyData();
        
        void UpdatePlayerData();

        void FilteringLobbySample();
    }
}