namespace NetCode.Lobby
{
    public interface ILobby
    {
        void CreateLobby();
        
        void DeleteAllCreatedLobbies();

        void DebugSMTH();
    }
}