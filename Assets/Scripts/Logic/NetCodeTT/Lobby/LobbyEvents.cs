using Unity.Services.Lobbies.Models;

namespace NetCodeTT.Lobbys
{
    public static class LobbyEvents
    {
        public delegate void LobbyUpdated(Lobby lobby);
        public static LobbyUpdated OnLobbyUpdated;
    }
}