using TTAuth;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class Initialization : MonoBehaviour
{
    private PlayerInfo _playerInfo;
    private Auth _auth = new Auth();
    public PlayerInfoClass _playerInfoClass = new PlayerInfoClass();

    public static Initialization Instance { get; private set; }
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
        DontDestroyOnLoad(this);
    }
    
    async void Start()
    {
        await UnityServices.InitializeAsync();
        Debug.Log($"Unity services initialization: {UnityServices.State}");

        //Shows if a cached session token exist
        Debug.Log($"Cached Session Token Exist: {AuthenticationService.Instance.SessionTokenExists}");

        // Shows Current profile
        Debug.Log($"Profile: {AuthenticationService.Instance.Profile}");
        PlayerData.SetNickname(AuthenticationService.Instance.Profile);
        
        _auth.Subscribe();
        _auth.SignInAnonymous();
    }
}
