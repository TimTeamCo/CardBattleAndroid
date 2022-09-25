using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private Button _playerNameButton;
    [SerializeField] private Text _playerName;
    [SerializeField] private Text _playerID;
    private void Start()
    {
        _playerNameButton.onClick.AddListener(OnClickNickname);
        UpdateData();
    }

    private void OnClickNickname()
    {
        
    }

    private void UpdateData()
    {
        Debug.Log("UpdateData call");
        var isSignedIn = AuthenticationService.Instance.IsSignedIn;
        if (isSignedIn)
        {
            PlayerData.SetPlayerId(AuthenticationService.Instance.PlayerId);
            PlayerData.SetNickname(AuthenticationService.Instance.Profile);
            _playerName.text = PlayerData.Nickname;
            _playerID.text = PlayerData.Id;
        }

        Debug.Log(isSignedIn ? "Signed in" : "Signed out");
        Debug.Log(isSignedIn ? GetPlayerInfoText(Initialization.Instance._playerInfoClass._playerInfo) : "");
        Debug.Log(AuthenticationService.Instance.SessionTokenExists ? "Session Token: Cached token exists" : "Session Token: Not Found");
        Debug.Log("Anonymous auth end");
    }
    
    string GetPlayerInfoText(PlayerInfo playerInfo)
    {
        if (playerInfo.CreatedAt == null)
            return string.Empty;

        var localDateTime = playerInfo?.CreatedAt.Value.ToLocalTime();

        var playerText = $"CreatedAt: {localDateTime.Value} \n ExternalIds: {Initialization.Instance._playerInfoClass._externalIds} \n ";

        return playerText;
    }
}