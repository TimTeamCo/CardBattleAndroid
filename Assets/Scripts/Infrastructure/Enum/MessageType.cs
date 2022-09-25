/// Ensure that message contents are obvious but not dependent on spelling strings correctly.
public enum MessageType
{
    // These are assigned arbitrary explicit values so that if a MessageType is serialized and more enum values are later inserted/removed, the serialized values need not be reassigned.
    // (If you want to remove a message, make sure it isn't serialized somewhere first.)
    None = 0,
    RenameRequest = 1,
    JoinLobbyRequest = 2,
    CreateLobbyRequest = 3,
    QueryLobbies = 4,
    QuickJoin = 5,

    ChangeMenuState = 100,
    ConfirmInGameState = 101,
    LobbyUserStatus = 102,
    UserSetEmote = 103,
    ClientUserApproved = 104,
    ClientUserSeekingDisapproval = 105,
    EndGame = 106,

    StartCountdown = 200,
    CancelCountdown = 201,
    CompleteCountdown = 202,
    MinigameBeginning = 203,
    InstructionsShown = 204,
    MinigameEnding = 205,

    DisplayErrorPopup = 300,
}
