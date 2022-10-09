namespace TTLobbyLogic
{
    public struct LobbyData
    {
        public string LobbyID { get; set; }
        public string LobbyCode { get; set; }
        public string RelayCode { get; set; }
        public string RelayNGOCode { get; set; }
        public string LobbyName { get; set; }
        public bool Private { get; set; }
        public int MaxPlayerCount { get; set; }
        public LobbyState State { get; set; }
        public LobbyColor Color { get; set; }
        public long State_LastEdit { get; set; }
        public long Color_LastEdit { get; set; }
        public long RelayNGOCode_LastEdit { get; set; }

        public LobbyData(LobbyData existing)
        {
            LobbyID = existing.LobbyID;
            LobbyCode = existing.LobbyCode;
            RelayCode = existing.RelayCode;
            RelayNGOCode = existing.RelayNGOCode;
            LobbyName = existing.LobbyName;
            Private = existing.Private;
            MaxPlayerCount = existing.MaxPlayerCount;
            State = existing.State;
            Color = existing.Color;
            State_LastEdit = existing.State_LastEdit;
            Color_LastEdit = existing.Color_LastEdit;
            RelayNGOCode_LastEdit = existing.RelayNGOCode_LastEdit;
        }

        public LobbyData(string lobbyCode)
        {
            LobbyID = null;
            LobbyCode = lobbyCode;
            RelayCode = null;
            RelayNGOCode = null;
            LobbyName = null;
            Private = false;
            MaxPlayerCount = -1;
            State = LobbyState.Lobby;
            Color = LobbyColor.None;
            State_LastEdit = 0;
            Color_LastEdit = 0;
            RelayNGOCode_LastEdit = 0;
        }
    }
}