using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

/// The Authentication package will sign in asynchronously and anonymously. When complete, we will need to store the generated ID.
public class SubIdentity_Authentication : SubIdentity, IDisposable
{
    private bool m_hasDisposed;

    /// This will kick off a login.
    public SubIdentity_Authentication(Action onSigninComplete = null)
    {
        DoSignIn(onSigninComplete);
    }
    ~SubIdentity_Authentication()
    {
        Dispose();
    }
    
    private async void DoSignIn(Action onSigninComplete)
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += OnSignInChange;
        AuthenticationService.Instance.SignedOut += OnSignInChange;

        try
        {   if (!AuthenticationService.Instance.IsSignedIn)
            {
                // Don't sign out later, since that changes the anonymous token, which would prevent the player from exiting lobbies they're already in.
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            onSigninComplete?.Invoke();
        }
        catch
        {   Debug.LogError("Failed to login. Did you remember to set your Project ID under Services > General Settings?");
            throw;
        }

        // Note: If for some reason your login state gets weird, you can comment out the previous block and instead call AuthenticationService.Instance.SignOut().
        // Then, running Play mode will fail to actually function and instead will log out of your previous anonymous account.
        // When you revert that change and run Play mode again, you should be logged in as a new anonymous account with a new default name.
    }
    
    private void OnSignInChange()
    {
        SetContent("id", AuthenticationService.Instance.PlayerId);
    }

    public void Dispose()
    {
        if (!m_hasDisposed)
        {
            AuthenticationService.Instance.SignedIn -= OnSignInChange;
            AuthenticationService.Instance.SignedOut -= OnSignInChange;
            m_hasDisposed = true;
        }
    }
}
