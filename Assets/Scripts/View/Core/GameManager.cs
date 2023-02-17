using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logic.Game;
using NetCodeTT.Lobby;
using Saver;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

[Flags]
public enum GameState
{
    Menu = 1,
    Lobby = 2,
    Game = 4,
}

public class GameManager : MonoBehaviour
{
    public Action onApplicationEntry;
    public Action onPressStartButton;
    public Action onExitSearchingButton;
    public Action onJoinIntoLobby;
    public LocalLobby LocalLobby => _localLobby;
    public Action<GameState> onGameStateChanged;
    public LocalLobbyList LobbyList { get; private set; } = new ();
    public GameState LocalGameState { get; private set; }
    
    // [SerializeField] SetupInGame m_setupInGame;
    // [SerializeField] Countdown m_countdown;

    LocalPlayer _localUser;
    LocalLobby _localLobby;

    LobbyColor m_lobbyColorFilter;

    private LobbyManager _lobbyManager;
    
    #region Setup

    public void Init()
    {
        if (String.IsNullOrEmpty(LocalSaver.GetPlayerNickname()))
        {
            ApplicationController.Instance._welcomeWindow.ShowWindow();
        }
        else
        {
            CreateLocalData();
        }

        _lobbyManager = ApplicationController.Instance.LobbyManager;
        Subscribe();
    }

    public void CreateLocalData()
    {
        _localUser = new LocalPlayer("", 0, false, "No name yet");
        _localLobby = new LocalLobby {LocalLobbyState = {Value = LobbyState.Lobby}};
        AuthenticatePlayer();
        LoadMenuScene();
    }

    void AuthenticatePlayer()
    {
        var localId = AuthenticationService.Instance.PlayerId;

        _localUser.ID.Value = localId;
        _localUser.DisplayName.Value = LocalSaver.GetPlayerNickname();
    }
    
    #endregion

    #region Subscribe

    private void Subscribe()
    {
        onPressStartButton = OnPressStartButton;
        onExitSearchingButton = OnExitSearchingButton;
    }

    private void OnExitSearchingButton()
    {
        SetLocalUserStatus(PlayerStatus.Menu);
        _lobbyManager.LeaveLobby();
    }

    private async void OnPressStartButton()
    {
        var lobby = await _lobbyManager.QuickJoin(_localUser); 
        if (lobby != null)
        {
            LobbyConverters.RemoteToLocal(lobby, _localLobby);
            onJoinIntoLobby?.Invoke();
            Debug.Log($"[Tim] _localUser.IsHost {_localUser.IsHost.Value }");
            await JoinLobby();
        }
        else
        {
            SetGameState(GameState.Menu);
        }
    }

    #endregion
    
    
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(1);
    }
    
    /// <summary>Rather than a setter, this is usable in-editor. It won't accept an enum, however.</summary>
    public void SetLobbyColorFilter(int color)
    {
        m_lobbyColorFilter = (LobbyColor) color;
    }

    public async Task<LocalPlayer> AwaitLocalUserInitialization()
    {
        while (_localUser == null)
            await Task.Delay(100);
        return _localUser;
    }

    public async void CreateLobby(string name, bool isPrivate, int maxPlayers = 4)
    {
        try
        {
            // var lobby = await LobbyManager.CreateLobbyAsync(
                // name,
                // maxPlayers,
                // isPrivate, m_LocalUser);

            // LobbyConverters.RemoteToLocal(lobby, m_LocalLobby);
            await CreateLobby();
        }
        catch (Exception exception)
        {
            // SetGameState(GameState.JoinMenu);
            Debug.LogError($"Error creating lobby : {exception} ");
        }
    }

    public async void JoinLobby(string lobbyID, string lobbyCode)
    {
        try
        {
            // var lobby = await LobbyManager.JoinLobbyAsync(lobbyID, lobbyCode,
                // m_LocalUser);

            // LobbyConverters.RemoteToLocal(lobby, m_LocalLobby);
            await JoinLobby();
        }
        catch (Exception exception)
        {
            // SetGameState(GameState.JoinMenu);
            Debug.LogError($"Error joining lobby : {exception} ");
        }
    }

    public async void QueryLobbies()
    {
        LobbyList.QueryState.Value = LobbyQueryState.Fetching;
        // var qr = await LobbyManager.RetrieveLobbyListAsync(m_lobbyColorFilter);
        // if (qr == null)
        {
            return;
        }

        // SetCurrentLobbies(LobbyConverters.QueryToLocalList(qr));
    }

    public async void QuickJoin()
    {
        // var lobby = await LobbyManager.QuickJoinLobbyAsync(m_LocalUser, m_lobbyColorFilter);
        // if (lobby != null)
        {
            // LobbyConverters.RemoteToLocal(lobby, m_LocalLobby);
            await JoinLobby();
        }
        // else
        {
            // SetGameState(GameState.JoinMenu);
        }
    }

    public void SetLocalUserName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            // LogHandlerSettings.Instance.SpawnErrorPopup(
                // "Empty Name not allowed."); // Lobby error type, then HTTP error type.
            return;
        }

        _localUser.DisplayName.Value = name;
        SendLocalUserData();
    }

    // public void SetLocalUserEmote(EmoteType emote)
    // {
        // m_LocalUser.Emote.Value = emote;
        // SendLocalUserData();
    // }

    public void SetLocalUserStatus(PlayerStatus status)
    {
        _localUser.UserStatus.Value = status;
        SendLocalUserData();
    }

    public void SetLocalLobbyColor(int color)
    {
        if (_localLobby.PlayerCount < 1)
            return;
        // _localLobby.LocalLobbyColor.Value = (LobbyColor) color;
        SendLocalLobbyData();
    }

    bool updatingLobby;

    async void SendLocalLobbyData()
    {
        // await LobbyManager.UpdateLobbyDataAsync(LobbyConverters.LocalToRemoteLobbyData(m_LocalLobby));
    }

    async void SendLocalUserData()
    {
        // await LobbyManager.UpdatePlayerDataAsync(LobbyConverters.LocalToRemoteUserData(m_LocalUser));
    }

    public void UIChangeMenuState(GameState state)
    {
        var isQuittingGame = LocalGameState == GameState.Lobby &&
                             _localLobby.LocalLobbyState.Value == LobbyState.InGame;

        if (isQuittingGame)
        {
            //If we were in-game, make sure we stop by the lobby first
            state = GameState.Lobby;
            ClientQuitGame();
        }

        SetGameState(state);
    }

    public void HostSetRelayCode(string code)
    {
        _localLobby.RelayCode.Value = code;
        SendLocalLobbyData();
    }

    //Only Host needs to listen to this and change state.
    void OnPlayersReady(int readyCount)
    {
        if (readyCount == _localLobby.PlayerCount &&
            _localLobby.LocalLobbyState.Value != LobbyState.CountDown)
        {
            _localLobby.LocalLobbyState.Value = LobbyState.CountDown;
            SendLocalLobbyData();
        }
        else if (_localLobby.LocalLobbyState.Value == LobbyState.CountDown)
        {
            _localLobby.LocalLobbyState.Value = LobbyState.Lobby;
            SendLocalLobbyData();
        }
    }

    void OnLobbyStateChanged(LobbyState state)
    {
        if (state == LobbyState.Lobby)
            CancelCountDown();
        if (state == LobbyState.CountDown)
            BeginCountDown();
    }

    void BeginCountDown()
    {
        Debug.Log("Beginning Countdown.");
        // m_countdown.StartCountDown();
    }

    void CancelCountDown()
    {
        Debug.Log("Countdown Cancelled.");
        // m_countdown.CancelCountDown();
    }

    public void FinishedCountDown()
    {
        _localUser.UserStatus.Value = PlayerStatus.InGame;
        _localLobby.LocalLobbyState.Value = LobbyState.InGame;
        // m_setupInGame.StartNetworkedGame(m_LocalLobby, m_LocalUser);
    }

    public void BeginGame()
    {
        if (_localUser.IsHost.Value)
        {
            _localLobby.LocalLobbyState.Value = LobbyState.InGame;
            _localLobby.Locked.Value = true;
            SendLocalLobbyData();
        }
    }

    public void ClientQuitGame()
    {
        EndGame();
        // m_setupInGame?.OnGameEnd();
    }

    public void EndGame()
    {
        if (_localUser.IsHost.Value)
        {
            _localLobby.LocalLobbyState.Value = LobbyState.Lobby;
            _localLobby.Locked.Value = false;
            SendLocalLobbyData();
        }

        SetLobbyView();
    }

    

    void SetGameState(GameState state)
    {
        // var isLeavingLobby = (state == GameState.Menu || state == GameState.JoinMenu) &&
                             // LocalGameState == GameState.Lobby;
        LocalGameState = state;

        Debug.Log($"Switching Game State to : {LocalGameState}");

        // if (isLeavingLobby)
        // {
            // LeaveLobby();
        // }

        onGameStateChanged?.Invoke(LocalGameState);
    }

    void SetCurrentLobbies(IEnumerable<LocalLobby> lobbies)
    {
        var newLobbyDict = new Dictionary<string, LocalLobby>();
        foreach (var lobby in lobbies)
            newLobbyDict.Add(lobby.LobbyID.Value, lobby);

        LobbyList.CurrentLobbies = newLobbyDict;
        LobbyList.QueryState.Value = LobbyQueryState.Fetched;
    }

    async Task CreateLobby()
    {
        _localUser.IsHost.Value = true;
        _localLobby.onUserReadyChange = OnPlayersReady;
        try
        {
            await BindLobby();
        }
        catch (Exception exception)
        {
            Debug.LogError($"Couldn't join Lobby: {exception}");
        }
    }

    async Task JoinLobby()
    {
        //Trigger UI Even when same value
        _localUser.IsHost.ForceSet(false);
        Debug.Log($"[Tim] _localUser.IsHost {_localUser.IsHost.Value }");
        await BindLobby();
    }

    async Task BindLobby()
    {
        // await LobbyManager.BindLocalLobbyToRemote(m_LocalLobby.LobbyID.Value, m_LocalLobby);
        _localLobby.LocalLobbyState.onChanged += OnLobbyStateChanged;
        SetLobbyView();
    }

    public void LeaveLobby()
    {
        _localUser.ResetState();
#pragma warning disable 4014
        // LobbyManager.LeaveLobbyAsync();
#pragma warning restore 4014
        ResetLocalLobby();
    }

    IEnumerator RetryConnection(Action doConnection, string lobbyId)
    {
        yield return new WaitForSeconds(5);
        if (_localLobby != null && _localLobby.LobbyID.Value == lobbyId && !string.IsNullOrEmpty(lobbyId)
           ) // Ensure we didn't leave the lobby during this waiting period.
            doConnection?.Invoke();
    }

    void SetLobbyView()
    {
        Debug.Log($"Setting Lobby user state {PlayerStatus.Lobby}");
        // SetGameState(GameState.Menu);
        SetLocalUserStatus(PlayerStatus.Lobby);
    }

    void ResetLocalLobby()
    {
        _localLobby.ResetLobby();
        _localLobby.RelayServer = null;
    }

    #region Teardown

    /// <summary>
    /// In builds, if we are in a lobby and try to send a Leave request on application quit, it won't go through if we're quitting on the same frame.
    /// So, we need to delay just briefly to let the request happen (though we don't need to wait for the result).
    /// </summary>
    IEnumerator LeaveBeforeQuit()
    {
        ForceLeaveAttempt();
        yield return null;
        Application.Quit();
    }

    public bool OnWantToQuit()
    {
        bool canQuit = string.IsNullOrEmpty(_localLobby?.LobbyID.Value);
        StartCoroutine(LeaveBeforeQuit());
        return canQuit;
    }

    void OnDestroy()
    {
        ForceLeaveAttempt();
        // LobbyManager.Dispose();
    }

    void ForceLeaveAttempt()
    {
        if (!string.IsNullOrEmpty(_localLobby?.LobbyID.Value))
        {
#pragma warning disable 4014
            // LobbyManager.LeaveLobbyAsync();
#pragma warning restore 4014
            _localLobby = null;
        }
    }

    #endregion
}