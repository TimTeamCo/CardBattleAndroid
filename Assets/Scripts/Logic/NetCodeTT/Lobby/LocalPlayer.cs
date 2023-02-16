using System;
using System.Text;
using Logic.Infrastructure;

[Flags]
public enum PlayerStatus
{
    Lobby = 1, // User is in a lobby and connected to Relay.
    Ready = 2, // User has selected the ready button, to ready for the "game" to start.
    InGame = 4, // User is part of a "game" that has started.
    Menu = 8 // User is not in a lobby, in one of the main menus.
}

[Serializable]
public class LocalPlayer
{
    public CallbackValue<bool> IsHost = new CallbackValue<bool>(false);
    public CallbackValue<string> DisplayName = new CallbackValue<string>("");
    public CallbackValue<PlayerStatus> UserStatus = new CallbackValue<PlayerStatus>((PlayerStatus) 8);
    public CallbackValue<PetType> Pet = new CallbackValue<PetType>(PetType.Cat1);
    public CallbackValue<string> ID = new CallbackValue<string>("");
    public CallbackValue<int> Index = new CallbackValue<int>(0);

    public DateTime LastUpdated;

    public LocalPlayer(string id, int index, bool isHost, string displayName = default,
        PetType pet = default, PlayerStatus status = default)
    {
        ID.Value = id;
        IsHost.Value = isHost;
        Index.Value = index;
        DisplayName.Value = displayName;
        Pet.Value = pet;
        UserStatus.Value = status;
    }

    public void ResetState()
    {
        IsHost.Value = false;
        UserStatus.Value = PlayerStatus.Menu;
    }
    
    public string ToString()
    {
        StringBuilder sb = new StringBuilder("IsHost : ");
        sb.AppendLine(IsHost.Value.ToString());
        sb.Append("DisplayName: ");
        sb.AppendLine(DisplayName.Value);
        sb.Append("UserStatus: ");
        sb.AppendLine(UserStatus.Value.ToString());
        sb.Append("Pet: ");
        sb.AppendLine(Pet.Value.ToString());
        sb.Append("ID: ");
        sb.AppendLine(ID.Value.ToString());
        sb.Append("Index: ");
        sb.AppendLine(Index.Value.ToString());

        return sb.ToString();
    }
}