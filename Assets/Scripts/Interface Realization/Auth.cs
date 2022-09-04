using UniRx;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class Auth : IAuth
{
    public ReactiveProperty<AuthState> PlayerAuthState = new ReactiveProperty<AuthState>();
    public enum AuthState
    {
        SignedIn,
        SignedOut,
        SignInFailed
    }
    public void Subscribe()
    {
        AuthenticationService.Instance.SignedIn += async () =>
        {
            //Shows how to get a playerID
            Debug.Log($"PlayedID: {AuthenticationService.Instance.PlayerId}");

            //Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

            Debug.Log("Sign in anonymously succeeded!");
            PlayerAuthState.Value = AuthState.SignedIn;
            await Initialization.Instance._playerInfoClass.GetPlayerInfoAsync();
            SceneLoader.LoadScene("Main Scene");
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Signed Out!");
            PlayerAuthState.Value = AuthState.SignedOut;
        };
        
        //You can listen to events to display custom messages
        AuthenticationService.Instance.SignInFailed += errorResponse =>
        {
            Debug.LogError($"Sign in anonymously failed with error code: {errorResponse.ErrorCode}");
            PlayerAuthState.Value = AuthState.SignInFailed;
        };
    }

    public async void SignInAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"Sign in anonymously failed with error code: {ex.ErrorCode}");
            Debug.LogError($"{ex.GetType().Name}: {ex.Message}");
        }
    }
}