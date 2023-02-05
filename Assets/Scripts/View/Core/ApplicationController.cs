using DG.Tweening;
using Logic.Connection;
using NetCodeTT.Authentication;
using NetCodeTT.Lobby;
using Unity.Services.Core;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    public static ApplicationController Instance { get; private set; }
    public LobbyManager LobbyManager { get; private set; }
    public IAuth AuthenticationManager { get; private set; }
    public IConnection ConnectionManager { get; private set; }
    
    public GameManager GameManager { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this);
        BindManagers();
        DOTween.Init();
    }

    private void BindManagers()
    {
        ConnectionManager = gameObject.AddComponent<ConnectionManager>();
        ConnectionManager.Init();
        AuthenticationManager = new AuthenticationManager();
        LobbyManager = gameObject.AddComponent<LobbyManager>();
        GameManager = new GameManager();
        Debug.Log($"Binded managers");
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);
        AuthenticationManager.SetupEvents();
        await AuthenticationManager.SignInAnonymouslyAsync();
    }
    
    private void OnApplicationQuit()
    {
        LobbyManager.LeaveLobby();
        StopAllCoroutines();
    }
}