using DG.Tweening;
using Logic.Connection;
using NetCodeTT.Authentication;
using NetCodeTT.Lobbys;
using UnityEngine;
using UnityEngine.SceneManagement;
using View.Core;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] public AudioController AudioController;
    [SerializeField] public GameObject _welcomeWindow;
    [SerializeField] public Countdown _countdown;
    public static ApplicationController Instance { get; private set; }
    public LobbyManager LobbyManager { get; private set; }
    public IAuth AuthenticationManager { get; private set; }
    public IConnection ConnectionManager { get; private set; }
    public GameManager GameManager { get; private set; }
    public GameLobbyManager GameLobbyManager { get; private set; }

    private bool isEntry = true;

    #region CheatPanel

    //Cheat button for dev and qa
    public void Debug1()
    {
    }
    
    //Cheat button for dev and qa
    public void Debug2()
    {
        LobbyManager.PrintPlayers();
    }

    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(_countdown);
        BindManagers();
    }

    private void BindManagers()
    {
        ConnectionManager = gameObject.AddComponent<ConnectionManager>();
        AuthenticationManager = new AuthenticationManager();
        LobbyManager = gameObject.AddComponent<LobbyManager>();
        GameManager = gameObject.AddComponent<GameManager>();
        GameLobbyManager = gameObject.AddComponent<GameLobbyManager>();
        GameLobbyManager.Init(LobbyManager);
    }

    private void Start()
    {
        DOTween.Init();
        ConnectionManager.Init();
        AuthenticationManager.Init();
        GameManager.Init();

        Subscribe();
    }

    private void Subscribe()
    {
        Application.wantsToQuit += OnWantToQuit;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" && isEntry)
        {
            isEntry = false;
            GameManager.onApplicationEntry?.Invoke();
        }
    }

    private bool OnWantToQuit()
    {
        return GameManager.OnWantToQuit();
    }
    
    private void OnApplicationQuit()
    {
        LobbyManager.LeaveLobby();
        StopAllCoroutines();
        DOTween.KillAll();
    }
}