namespace TTGame
{
    /// Used when displaying the lobby list, to indicate when we are awaiting an updated lobby query.
    public enum LobbyQueryState
    {
        Empty,
        Fetching,
        Error,
        Fetched
    }
}