using System;
using System.Collections;
using System.Collections.Generic;
using TTAuth;
using TTLobbyLogic;
using TTRelay;
using UnityEngine;

namespace TTGame
{
    /// All the Data that is important gets updated in here, the GameManager in the mainScene has all the references
    /// needed to run the game.
    public class GameManager : MonoBehaviour, IReceiveMessages
    {
        #region UI elements that observe the local state. These should be assigned the observers in the scene during Start.

        /// <summary>
        /// The Observer/Observed Pattern is great for keeping the UI in Sync with the actual Values.
        /// Each list below represents a single Observed class that gets updated by other parts of the code, and will
        /// trigger the list of Observers that are looking for changes in that class.
        ///
        /// The list is serialized, so you can navigate to the Observers via the Inspector to see who's watching.
        /// </summary>
        [SerializeField]
        private List<LocalMenuStateObserver> m_LocalGameStateObservers = new List<LocalMenuStateObserver>();

        [SerializeField] private List<LocalLobbyObserver> m_LocalLobbyObservers = new List<LocalLobbyObserver>();
        [SerializeField] private List<LobbyUserObserver> m_LocalUserObservers = new List<LobbyUserObserver>();

        [SerializeField]
        private List<LobbyServiceDataObserver> m_LobbyServiceObservers = new List<LobbyServiceDataObserver>();

        #endregion

        private LocalGameState m_LocalGameState = new LocalGameState();
        private LobbyUser m_localUser;
        private LocalLobby m_localLobby;
        private LobbyServiceData m_lobbyServiceData = new LobbyServiceData();

        private LobbyColor m_lobbyColorFilter;

        private LobbyContentHeartbeat m_lobbyContentHeartbeat = new LobbyContentHeartbeat();

        private RelayUtpSetup m_relaySetup;
        private RelayUtpClient m_relayClient;

        private bool gameReady;

        //Rather than a setter, this is usable in-editor. It won't accept an enum, however.
        public void SetLobbyColorFilter(int color)
        {
            m_lobbyColorFilter = (LobbyColor) color;
        }

        #region Setup

        private void Awake()
        {
            StartCoroutine(LoadResources());
            
            // Do some arbitrary operations to instantiate singletons.
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var unused = Locator.Get;
#pragma warning restore IDE0059

            Locator.Get.Provide(new Identity(OnAuthSignIn));
            Application.wantsToQuit += OnWantToQuit;
        }

        private IEnumerator LoadResources()
        {
            yield return new WaitUntil(() => gameReady);
            SetGameState(GameState.Menu);
        }

        private void Start()
        {
            m_localLobby = new LocalLobby {State = LobbyState.Lobby};
            m_localUser = new LobbyUser();
            m_localUser.DisplayName = "New Player";
            Locator.Get.Messenger.Subscribe(this);
            BeginObservers();
        }

        private void OnAuthSignIn()
        {
            Debug.Log("Signed in.");
            m_localUser.ID = Locator.Get.Identity.GetSubIdentity(Auth.IIdentityType.Auth).GetContent("id");
            Debug.Log($"m_localUser.ID {m_localUser.ID}.");

            m_localUser.DisplayName = NameGenerator.GetName(m_localUser.ID);
            Debug.Log($"m_localUser.DisplayName {m_localUser.DisplayName}.");
            // The local LobbyUser object will be hooked into UI before the LocalLobby is populated during lobby join,
            // so the LocalLobby must know about it already when that happens.
            m_localLobby.AddPlayer(m_localUser);
            gameReady = true;
        }

        private void BeginObservers()
        {
            foreach (var gameStateObs in m_LocalGameStateObservers)
                gameStateObs.BeginObserving(m_LocalGameState);
            foreach (var serviceObs in m_LobbyServiceObservers)
                serviceObs.BeginObserving(m_lobbyServiceData);
            foreach (var lobbyObs in m_LocalLobbyObservers)
                lobbyObs.BeginObserving(m_localLobby);
            foreach (var userObs in m_LocalUserObservers)
                userObs.BeginObserving(m_localUser);
        }

        #endregion

        /// The Messaging System handles most of the core Lobby Service calls, and catches the callbacks from those calls.
        /// These In turn update the observed variables and propagates the events to the game.
        /// When looking for the interactions, look up the MessageType and search for it in the code to see where it is used outside this script.
        /// EG. Locator.Get.Messenger.OnReceiveMessage(MessageType.RenameRequest, name);
        public void OnReceiveMessage(MessageType type, object msg)
        {
            if (type == MessageType.CreateLobbyRequest)
            {
                CreateLobby(msg);
            }
            else if (type == MessageType.JoinLobbyRequest)
            {
                LobbyData lobbyInfo = (LobbyData) msg;
                LobbyAsyncRequests.Instance.JoinLobbyAsync(lobbyInfo.LobbyID, lobbyInfo.LobbyCode, m_localUser, (r) =>
                    {
                        ToLocalLobby.Convert(r, m_localLobby);
                        OnJoinedLobby();
                    },
                    OnFailedJoin);
            }
            else if (type == MessageType.QueryLobbies)
            {
                m_lobbyServiceData.State = LobbyQueryState.Fetching;
                LobbyAsyncRequests.Instance.RetrieveLobbyListAsync(
                    qr => {
                        if (qr != null)
                        {
                            OnLobbiesQueried(ToLocalLobby.Convert(qr));
                        }
                    },
                    er => {
                        OnLobbyQueryFailed();
                    },
                    m_lobbyColorFilter);
            }
            else if (type == MessageType.QuickJoin)
            {
                Debug.Log($"create or join to lobby");
                //create or join to lobby
                LobbyAsyncRequests.Instance.QuickJoinLobbyAsync(m_localUser, m_lobbyColorFilter, (lobby) =>
                    {
                        {
                            ToLocalLobby.Convert(lobby, m_localLobby);
                        }
                        OnJoinedLobby();
                    },
                    () =>
                    {
                        Debug.Log("Can't join to lobby");
                        //create lobby
                        LobbyData m_ServerRequestData = new LobbyData { LobbyName = "New Lobby", MaxPlayerCount = 2 };
                        CreateLobby(m_ServerRequestData);
                    });
            }
            else if (type == MessageType.RenameRequest)
            {
                string name = (string) msg;
                if (string.IsNullOrWhiteSpace(name))
                {
                    // Lobby error type, then HTTP error type.
                    Locator.Get.Messenger.OnReceiveMessage(MessageType.DisplayErrorPopup, "Empty Name not allowed.");
                    return;
                }

                m_localUser.DisplayName = (string) msg;
            }
            else if (type == MessageType.ClientUserApproved)
            {
                ConfirmApproval();
            }
            else if (type == MessageType.UserSetEmote)
            {
                EmoteType emote = (EmoteType) msg;
                m_localUser.Emote = emote;
            }
            else if (type == MessageType.LobbyUserStatus)
            {
                m_localUser.UserStatus = (UserStatus) msg;
            }
            else if (type == MessageType.StartCountdown)
            {
                m_localLobby.State = LobbyState.CountDown;
            }
            else if (type == MessageType.CancelCountdown)
            {
                m_localLobby.State = LobbyState.Lobby;
            }
            else if (type == MessageType.CompleteCountdown)
            {
                if (m_relayClient is RelayUtpHost)
                {
                    (m_relayClient as RelayUtpHost).SendInGameState();
                }
            }
            else if (type == MessageType.ChangeMenuState)
            {
                SetGameState((GameState) msg);
            }
            else if (type == MessageType.ConfirmInGameState)
            {
                m_localUser.UserStatus = UserStatus.InGame;
                m_localLobby.State = LobbyState.InGame;
            }
            else if (type == MessageType.EndGame)
            {
                m_localLobby.State = LobbyState.Lobby;
                SetUserLobbyState();
            }
        }

        private void CreateLobby(object msg)
        {
            LobbyData createLobbyData = (LobbyData) msg;
            LobbyAsyncRequests.Instance.CreateLobbyAsync(createLobbyData.LobbyName, createLobbyData.MaxPlayerCount,
                createLobbyData.Private, m_localUser, (r) =>
                {
                    ToLocalLobby.Convert(r, m_localLobby);
                    OnCreatedLobby();
                },
                OnFailedJoin);
        }

        private void SetGameState(GameState state)
        {
            bool isLeavingLobby = (state == GameState.Menu || state == GameState.Loading) && m_LocalGameState.State == GameState.Game;
            m_LocalGameState.State = state;
            if (isLeavingLobby)
            {
                OnLeftLobby();
            }
        }
        
        private void SetUserLobbyState()
        {
            SetGameState(GameState.Game);
            OnReceiveMessage(MessageType.LobbyUserStatus, UserStatus.Lobby);
        }
        
        private void ResetLocalLobby()
        {
            m_localLobby.CopyObserved(new LobbyData(), new Dictionary<string, LobbyUser>());
            // As before, the local player will need to be plugged into UI before the lobby join actually happens.
            m_localLobby.AddPlayer(m_localUser);
            m_localLobby.RelayServer = null;
        }
        
        private void OnCreatedLobby()
        {
            Debug.Log("Create Lobby");
            m_localUser.IsHost = true;
            OnJoinedLobby();
        }
        
        private void OnLobbiesQueried(IEnumerable<LocalLobby> lobbies)
        {
            var newLobbyDict = new Dictionary<string, LocalLobby>();
            foreach (var lobby in lobbies)
            {
                newLobbyDict.Add(lobby.LobbyID, lobby);
            }

            m_lobbyServiceData.State = LobbyQueryState.Fetched;
            m_lobbyServiceData.CurrentLobbies = newLobbyDict;
        }
        
        private void ConfirmApproval()
        {
            if (!m_localUser.IsHost && m_localUser.IsApproved)
            {
                CompleteRelayConnection();
            }
        }
        
        private void CompleteRelayConnection()
        {
            OnReceiveMessage(MessageType.LobbyUserStatus, UserStatus.Lobby);
        }
        
        private void OnLobbyQueryFailed()
        {
            m_lobbyServiceData.State = LobbyQueryState.Error;
        }
        
        private void OnLeftLobby()
        {
            m_localUser.ResetState();
            LobbyAsyncRequests.Instance.LeaveLobbyAsync(m_localLobby.LobbyID, ResetLocalLobby);
            // m_lobbyContentHeartbeat.EndTracking();
            LobbyAsyncRequests.Instance.EndTracking();

            if (m_relaySetup != null)
            {   
                Destroy(m_relaySetup);
                m_relaySetup = null;
            }
            if (m_relayClient != null)
            {
                m_relayClient.Dispose();
                StartCoroutine(FinishCleanup());

                // We need to delay slightly to give the disconnect message sent during Dispose time to reach the host, so that we don't destroy the connection without it being flushed first.
                IEnumerator FinishCleanup()
                {
                    yield return null;
                    Destroy(m_relayClient);
                    m_relayClient = null;
                }
            }
        }
        
        // Back to Join menu if we fail to join for whatever reason.
        private void OnFailedJoin()
        {
            Debug.Log("FailedJoin");
            SetGameState(GameState.Menu);
        }
        
        private void OnJoinedLobby()
        {
            Debug.Log("Join Lobby");
            LobbyAsyncRequests.Instance.BeginTracking(m_localLobby.LobbyID);
            m_lobbyContentHeartbeat.BeginTracking(m_localLobby, m_localUser);
            SetUserLobbyState();

            // The host has the opportunity to reject incoming players, but to do so the player needs to connect to Relay without having game logic available.
            // In particular, we should prevent players from joining voice chat until they are approved.
            OnReceiveMessage(MessageType.LobbyUserStatus, UserStatus.Connecting);
            if (m_localUser.IsHost)
            {
                StartRelayConnection();
            }
            else
            {
                StartRelayConnection();
            }
        }
        
        private void StartRelayConnection()
        {
            if (m_localUser.IsHost)
            {
                m_relaySetup = gameObject.AddComponent<RelayUtpSetupHost>();
                Debug.Log("Relay Host");
            }
            else
            {
                m_relaySetup = gameObject.AddComponent<RelayUtpSetupClient>();
                Debug.Log("Relay Client");
            }
            m_relaySetup.BeginRelayJoin(m_localLobby, m_localUser, OnRelayConnected);

            void OnRelayConnected(bool didSucceed, RelayUtpClient client)
            {
                Destroy(m_relaySetup);
                m_relaySetup = null;

                if (didSucceed == false)
                {   Debug.LogError("Relay connection failed! Retrying in 5s...");
                    StartCoroutine(RetryConnection(StartRelayConnection, m_localLobby.LobbyID));
                    return;
                }

                m_relayClient = client;
                if (m_localUser.IsHost)
                {
                    Debug.Log($"Host is now {m_localUser.DisplayName}");
                    CompleteRelayConnection();
                }
                else
                {
                    Debug.Log("Client is now waiting for approval...");
                }
            }
        }
        
        private IEnumerator RetryConnection(Action doConnection, string lobbyId)
        {
            yield return new WaitForSeconds(5);
            if (m_localLobby != null && m_localLobby.LobbyID == lobbyId && !string.IsNullOrEmpty(lobbyId)) // Ensure we didn't leave the lobby during this waiting period.
                doConnection?.Invoke();
        }
        
        #region Teardown

        /// In builds, if we are in a lobby and try to send a Leave request on application quit, it won't go through if we're quitting on the same frame.
        /// So, we need to delay just briefly to let the request happen (though we don't need to wait for the result).
        private IEnumerator LeaveBeforeQuit()
        {
            ForceLeaveAttempt();
            yield return null;
            Application.Quit();
        }

        private bool OnWantToQuit()
        {
            bool canQuit = string.IsNullOrEmpty(m_localLobby?.LobbyID);
            StartCoroutine(LeaveBeforeQuit());
            return canQuit;
        }

        private void OnDestroy()
        {
            ForceLeaveAttempt();
        }

        private void ForceLeaveAttempt()
        {
            Locator.Get.Messenger.Unsubscribe(this);
            if (!string.IsNullOrEmpty(m_localLobby?.LobbyID))
            {
                LobbyAsyncRequests.Instance.LeaveLobbyAsync(m_localLobby?.LobbyID, null);
                m_localLobby = null;
            }
        }

        #endregion
    }
}