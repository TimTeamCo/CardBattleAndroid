using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyWindowView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _opponentName;
    [SerializeField] private Image _playerPet;
    [SerializeField] private Image _opponentPet;
    [SerializeField] Sprite[] _petIcons;
    LocalLobby _localLobby;
    LocalPlayer _localPlayer;
    LocalPlayer _opponent;
    public string UserId { get; set; }
    public string OpponentUserId { get; set; }

    public void Start()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _localLobby = ApplicationController.Instance.GameManager.LocalLobby;
        _localLobby.onUserJoined += OnUserJoined;
        _localLobby.onUserLeft += OnUserLeft;
        _localLobby.LocalLobbyState.onChanged += OnLobbyStateChanged;
    }

    private void OnLobbyStateChanged(LobbyState obj)
    {
        if (obj == LobbyState.CountDown)
        {
            ShowLobby();
        }
        else
        {
            HideLobby();
        }
    }
    
    private void ShowLobby()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void HideLobby()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
    }

    private void OnUserLeft(int obj)
    {
        SynchPlayerUI();
    }

    private void OnUserJoined(LocalPlayer obj)
    {
        SynchPlayerUI();
    }

    private void SynchPlayerUI()
    {
        ResetUI();
        for (int i = 0; i < _localLobby.PlayerCount; i++)
        {
            var player = _localLobby.GetLocalPlayer(i);
            if (player == null)
            {
                SetOpponentUser(player);
            }
            else
            {
                SetPlayerUser(player);
            }
        }
    }

    private void SetOpponentUser(LocalPlayer opponent)
    {
        _opponent = opponent;
        OpponentUserId = _opponent.ID.Value;
        Debug.Log($"[Tim] player is host {opponent.IsHost.Value}");
        SetOpponentPet(opponent.Pet.Value);
        Debug.Log($"[Tim] player UserStatus {opponent.UserStatus.Value}");
        SetOpponentDisplayName(_opponent.DisplayName.Value);
        SubscribeToOpponentUpdates();
    }
    
    public void SetPlayerUser(LocalPlayer localPlayer)
    {
        _localPlayer = localPlayer;
        UserId = localPlayer.ID.Value;
        Debug.Log($"[Tim] player is host {localPlayer.IsHost.Value}");
        SetPlayerPet(localPlayer.Pet.Value);
        Debug.Log($"[Tim] player UserStatus {localPlayer.UserStatus.Value}");
        SetPlayerDisplayName(_localPlayer.DisplayName.Value);
        SubscribeToPlayerUpdates();
    }

    public void ResetUI()
    {
        if (_localPlayer == null)
            return;
        UserId = null;
        OpponentUserId = null;
        SetPlayerPet(PetType.Cat1);
        SetOpponentPet(PetType.Cat1);
        UnsubscribeToPlayerUpdates();
        UnsubscribeToOpponentUpdates();
        _localPlayer = null;
        _opponent = null;
    }

    private void SubscribeToPlayerUpdates()
    {
        _localPlayer.Pet.onChanged += SetPlayerPet;
    }
    
    private void SubscribeToOpponentUpdates()
    {
        _opponent.Pet.onChanged += SetOpponentPet;
    }
    
    private void UnsubscribeToPlayerUpdates()
    {
        _localPlayer.Pet.onChanged -= SetPlayerPet;
    }
    
    private void UnsubscribeToOpponentUpdates()
    {
        _opponent.Pet.onChanged -= SetOpponentPet;
    }

    private void SetPlayerDisplayName(string displayNameValue)
    {
        _playerName.text = displayNameValue;
    }

    private void SetOpponentDisplayName(string displayNameValue)
    {
        _opponentName.text = displayNameValue;
    }

    private void SetPlayerPet(PetType petType)
    {   
        _playerPet.sprite = PetSprite(petType);
    }
    
    private void SetOpponentPet(PetType petType)
    {   
        _opponentPet.sprite = PetSprite(petType);
    }
    
    Sprite PetSprite(PetType type)
    {
        switch (type)
        {
            case PetType.Cat1:
                return _petIcons[0];
            case PetType.Cat2:
                return _petIcons[1];
            case PetType.Dog1:
                return _petIcons[2];
            case PetType.Dog2:
                return _petIcons[3];
            case PetType.Dog3:
                return _petIcons[4];
            default:
                return null;
        }
    }
}
