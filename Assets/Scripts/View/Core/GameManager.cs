using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCodeTT.Lobbys;
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
    public Action<PetType> onPetChange;
    public LocalLobby LocalLobby => _localLobby;
    public LocalPlayer LocalPlayer => _localUser;
    public Action<GameState> onGameStateChanged;
    public LocalLobbyList LobbyList { get; private set; } = new ();
    public GameState LocalGameState { get; private set; }
    
    private LocalPlayer _localUser;
    private LocalLobby _localLobby;
    private LobbyManager _lobbyManager;
    private Countdown _countdown;

    // LobbyColor m_lobbyColorFilter;

    #region Setup

    public void Init()
    {
        _lobbyManager = ApplicationController.Instance.LobbyManager;
        _countdown = ApplicationController.Instance._countdown;
        
        if (String.IsNullOrEmpty(LocalSaver.GetPlayerPet()))
        {
            LocalSaver.SetPlayerPet(PetType.Cat1);
        }

        if (String.IsNullOrEmpty(LocalSaver.GetPlayerNickname()))
        {
            var windowObject = Instantiate(ApplicationController.Instance._welcomeWindow);
            var welcomWindow = windowObject.GetComponent<WelcomeWindow>();
            welcomWindow.ShowWindow();
        }
        else
        {
            CreateLocalData();
        }
        
        Subscribe();
    }

    public void CreateLocalData()
    {
        _localUser = new LocalPlayer("",  false, "No name yet");
        _localLobby = new LocalLobby {LocalLobbyState = {Value = LobbyState.Lobby}};
        AuthenticatePlayer();
        LoadMenuScene();
    }

    void AuthenticatePlayer()
    {
        var localId = AuthenticationService.Instance.PlayerId;

        _localUser.ID.Value = localId;
        _localUser.PlayerName.Value = LocalSaver.GetPlayerNickname();
        SendLocalUserData();
    }
    
    #endregion

    #region Subscribe

    private void Subscribe()
    {
        onPressStartButton += OnPressStartButton;
        onExitSearchingButton += OnExitSearchingButton;
    }

    private void OnExitSearchingButton()
    {
        SetLocalUserStatus(PlayerStatus.Menu);
        _lobbyManager.LeaveLobby();
    }

    private async void OnPressStartButton()
    {
        var lobby = await _lobbyManager.QuickJoin(_localUser);
        bool isHost = _lobbyManager.IsHostUser();
        if (lobby != null)
        {
            LobbyConverters.RemoteToLocal(lobby, _localLobby);
            onJoinIntoLobby?.Invoke();
            if (isHost)
            {
                _localUser.IsHost.Value = true;
                Debug.Log($"LocalUser Is Host");
                _localLobby.onUserReadyChange = OnPlayersReady;
                await JoinLobby(true);
            }
            else
            {
                _localUser.IsHost.Value = false;
                Debug.Log($"LocalUser Isn't Host");
                
                //send update lobby info to host
                SendLocalLobbyData();
                await JoinLobby(false);
            }

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
    
    void SetGameState(GameState state)
    {
        var isLeavingLobby = state == GameState.Menu && LocalGameState == GameState.Lobby;
        LocalGameState = state;

        Debug.Log($"Switching Game State to : {LocalGameState}");

        if (isLeavingLobby)
        { 
            LeaveLobby();
        }

        onGameStateChanged?.Invoke(LocalGameState);
    }
    
    public void LeaveLobby()
    {
        _localUser.ResetState();
#pragma warning disable 4014
        _lobbyManager.LeaveLobbyAsync();
#pragma warning restore 4014
        ResetLocalLobby();
    }
    
    void ResetLocalLobby()
    {
        _localLobby.ResetLobby();
        _localLobby.RelayServer = null;
    }
    
    public void SetLocalUserStatus(PlayerStatus status)
    {
        _localUser.UserStatus.Value = status;
        Debug.Log($"Change PlayerStatus to {status}");
        SendLocalUserData();
    }
    
    async void SendLocalUserData()
    {
        if (_localUser == null)
        {
            return;
        }
        await _lobbyManager.UpdatePlayerDataAsync(LobbyConverters.LocalToRemoteUserData(_localUser));
        Debug.Log($"Send Local User Data");
    }
    
    private async Task JoinLobby(bool likeHost)
    {
        //Trigger UI Even when same value
        _localUser.IsHost.ForceSet(likeHost);
        await BindLobby();
    }
    
    //Only Host needs to listen to this and change state.
    private void OnPlayersReady(int readyCount)
    {
        Debug.Log($"On Players Ready we have:\n ready Count = {readyCount}\n _localLobby.PlayerCount = {_localLobby.MaxPlayerCount.Value}\n _localLobby.LocalLobbyState.Value = {_localLobby.LocalLobbyState.Value}");
        if (readyCount == _localLobby.MaxPlayerCount.Value && _localLobby.LocalLobbyState.Value != LobbyState.CountDown)
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
    
    private async void SendLocalLobbyData()
    {
        await _lobbyManager.UpdateLobbyDataAsync(LobbyConverters.LocalToRemoteLobbyData(_localLobby));
    }
    
    private async Task BindLobby()
    {
        await _lobbyManager.BindLocalLobbyToRemote(_localLobby.LobbyID.Value, _localLobby);
        _localLobby.LocalLobbyState.onChanged += OnLobbyStateChanged;
        // _localLobby.UpdatePlayer(LocalPlayer);
        SetLobbyView();
    }

    private void OnLobbyStateChanged(LobbyState state)
    {
        Debug.Log($"LobbyState change to {state}");
        if (state == LobbyState.Lobby)
            CancelCountDown();
        if (state == LobbyState.CountDown)
        {
            SetGameState(GameState.Lobby); 
            BeginCountDown();
        }
    }
    
    void CancelCountDown()
    {
        Debug.Log("Countdown Cancelled.");
        _countdown.CancelCountDown();
    }
    
    void BeginCountDown()
    {
        Debug.Log("Beginning Countdown.");
        _countdown.StartCountDown();
    }
    
    public void FinishedCountDown()
    {
        _localUser.UserStatus.Value = PlayerStatus.InGame;
        _localLobby.LocalLobbyState.Value = LobbyState.InGame;
        //TODO Start Network Game
        // m_setupInGame.StartNetworkedGame(m_LocalLobby, m_LocalUser);
    }
    
    void SetLobbyView()
    {
        SetGameState(GameState.Menu);
        SetLocalUserStatus(PlayerStatus.Lobby);
    }
    
    public void SetLocalPet(PetType pet)
    {
        _localUser.Pet.Value = pet;
        onPetChange?.Invoke(pet);
        SendLocalUserData();
        Debug.Log($"SetLocalPet: Pet - {_localUser.Pet.Value}");
    }
    
    #region Teardown

    public bool OnWantToQuit()
    {
        bool canQuit = string.IsNullOrEmpty(_localLobby?.LobbyID.Value);
        StartCoroutine(LeaveBeforeQuit());
        return canQuit;
    }

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

    void ForceLeaveAttempt()
    {
        if (!string.IsNullOrEmpty(_localLobby?.LobbyID.Value))
        {
#pragma warning disable 4014
            _lobbyManager.LeaveLobbyAsync();
#pragma warning restore 4014
            _localLobby = null;
        }
    }

    void OnDestroy()
    {
        ForceLeaveAttempt();
        _lobbyManager.Dispose();
    }

    #endregion

    //TODO Remove down unused element
    #region UnUsed
    
    /// <summary>Rather than a setter, this is usable in-editor. It won't accept an enum, however.</summary>
    // public void SetLobbyColorFilter(int color)
    // {
        // m_lobbyColorFilter = (LobbyColor) color;
    // }

    public async Task<LocalPlayer> AwaitLocalUserInitialization()
    {
        while (_localUser == null)
            await Task.Delay(100);
        return _localUser;
    }

    // public async void CreateLobby(string name, bool isPrivate, int maxPlayers = 4)
    // {
        // try
        // {
            // var lobby = await LobbyManager.CreateLobbyAsync(
                // name,
                // maxPlayers,
                // isPrivate, m_LocalUser);

            // LobbyConverters.RemoteToLocal(lobby, m_LocalLobby);
            // await CreateLobby();
        // }
        // catch (Exception exception)
        // {
            // SetGameState(GameState.JoinMenu);
            // Debug.LogError($"Error creating lobby : {exception} ");
        // }
    // }

    // public async void JoinLobby(string lobbyID, string lobbyCode)
    // {
        // try
        // {
            // var lobby = await LobbyManager.JoinLobbyAsync(lobbyID, lobbyCode,
                // m_LocalUser);

            // LobbyConverters.RemoteToLocal(lobby, m_LocalLobby);
            // await JoinLobby();
        // }
        // catch (Exception exception)
        // {
            // SetGameState(GameState.JoinMenu);
            // Debug.LogError($"Error joining lobby : {exception} ");
        // }
    // }

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

    // public async void QuickJoin()
    // {
        // var lobby = await LobbyManager.QuickJoinLobbyAsync(m_LocalUser, m_lobbyColorFilter);
        // if (lobby != null)
        // {
            // LobbyConverters.RemoteToLocal(lobby, m_LocalLobby);
            // await JoinLobby();
        // }
        // else
        // {
            // SetGameState(GameState.JoinMenu);
        // }
    // }


    public void SetLocalLobbyColor(int color)
    {
        if (_localLobby.PlayerCount < 1)
            return;
        // _localLobby.LocalLobbyColor.Value = (LobbyColor) color;
        SendLocalLobbyData();
    }

    bool updatingLobby;

    // public void UIChangeMenuState(GameState state)
    // {
        // var isQuittingGame = LocalGameState == GameState.Lobby && _localLobby.LocalLobbyState.Value == LobbyState.InGame;

        // if (isQuittingGame)
        // {
            //If we were in-game, make sure we stop by the lobby first
            // state = GameState.Lobby;
            // ClientQuitGame();
        // }

        // SetGameState(state);
    // }

    public void HostSetRelayCode(string code)
    {
        _localLobby.RelayCode.Value = code;
        SendLocalLobbyData();
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

    // public void ClientQuitGame()
    // {
        // EndGame();
        // m_setupInGame?.OnGameEnd();
    // }

    // public void EndGame()
    // {
        // if (_localUser.IsHost.Value)
        // {
            // _localLobby.LocalLobbyState.Value = LobbyState.Lobby;
            // _localLobby.Locked.Value = false;
            // SendLocalLobbyData();
        // }

        // SetLobbyView();
    // }

    void SetCurrentLobbies(IEnumerable<LocalLobby> lobbies)
    {
        var newLobbyDict = new Dictionary<string, LocalLobby>();
        foreach (var lobby in lobbies)
            newLobbyDict.Add(lobby.LobbyID.Value, lobby);

        LobbyList.CurrentLobbies = newLobbyDict;
        LobbyList.QueryState.Value = LobbyQueryState.Fetched;
    }

    // async Task CreateLobby()
    // {
        // _localUser.IsHost.Value = true;
        // _localLobby.onUserReadyChange = OnPlayersReady;
        // try
        // {
            // await BindLobby();
        // }
        // catch (Exception exception)
        // {
            // Debug.LogError($"Couldn't join Lobby: {exception}");
        // }
    // }
    
    IEnumerator RetryConnection(Action doConnection, string lobbyId)
    {
        yield return new WaitForSeconds(5);
        if (_localLobby != null && _localLobby.LobbyID.Value == lobbyId && !string.IsNullOrEmpty(lobbyId)
           ) // Ensure we didn't leave the lobby during this waiting period.
            doConnection?.Invoke();
    }
    
    #endregion
}