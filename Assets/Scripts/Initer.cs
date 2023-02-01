using System.Collections;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Initer : MonoBehaviour
{
    [SerializeField] private ViewPopup _viewPopup;

    void Start()
    {
        StartCoroutine(TestInternetConection());
    }

    private async void StartIniter()
    {
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);
        SetupEvents();
        await SignInAnonymouslyAsync();
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

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"SignInAnonymouslyAsync PlayerID: {AuthenticationService.Instance.PlayerId}");
            SceneManager.LoadScene(1);
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public IEnumerator TestInternetConection()
    {
        string[] _urls =
        {
            "https://www.google.com", "https://www.facebook.com", "https://www.wikipedia.org", "https://www.apple.com",
            "https://www.un.org"
        };

        while (true)
        {
            Debug.Log("Test Internet Conection");
            foreach (var url in _urls)
            {
                UnityWebRequest request = UnityWebRequest.Get(url);

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success) continue;
                Debug.Log("Is Internet");
                _viewPopup.ShowPopup(false);
                StartIniter();
                yield break;
            }

            Debug.Log("No Internet");
            _viewPopup.ShowPopup(true);
            yield return new WaitForSeconds(3f);
        }
    }
}