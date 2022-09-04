public static class PlayerData
{
    public static string Nickname { get; private set; }
    public static string Id { get; private set; }

    public static void SetNickname(string name)
    {
        Nickname = name;
    }
    
    public static void SetPlayerId(string id)
    {
        Id = id;
    }
}