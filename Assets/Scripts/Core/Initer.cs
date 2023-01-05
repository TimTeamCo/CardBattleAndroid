using NetCode.Authentication;
using NetCode.Lobby;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class Initer : MonoBehaviour
{
    public static Initer Instance { get; private set; }
    public ILobby LobbyManager { get; private set; }
    public IAuth AuthenticationManager { get; private set; }
    
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
    }

    private void BindManagers()
    {
        LobbyManager = gameObject.AddComponent<LobbyManager>();
        AuthenticationManager = new AuthenticationManager();
        Debug.Log($"Binded managers");
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);
        SetupEvents();
        await AuthenticationManager.SignInAnonymouslyAsync();
    }

    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Shows how to get a playerID
            Debug.Log($"SetupEvents PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
        };

        AuthenticationService.Instance.SignInFailed += (err) => { Debug.LogError(err); };

        AuthenticationService.Instance.SignedOut += () => { Debug.Log("Player signed out."); };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }
}