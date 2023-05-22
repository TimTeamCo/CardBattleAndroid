using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Core;

public class MainMenuWindow : MonoBehaviour, IDisposable
{
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _joinButton;
    [SerializeField] private ShopButtonView _shopButtonView;
    [SerializeField] private InventoryButtonView _inventoryButtonView;
    [SerializeField] private RectTransform _xpRect;
    [SerializeField] private RectTransform _dustRect;
    [SerializeField] private AudioClip _mainTheme;
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;
    [SerializeField] private TMP_InputField _lobbyCodeInput;
    
    private GameLobbyManager _lobbyManager;

    private void Awake()
    {
        ApplicationController.Instance.AudioController.SetBackgroundMusicSource(_mainTheme, true, true);
        _lobbyManager = ApplicationController.Instance.GameLobbyManager;
    }
    
    private void OnEnable()
    {
        _shopButton.onClick.AddListener(OnClickShopButton);
        _inventoryButton.onClick.AddListener(OnClickInventoryButton);
        _hostButton.onClick.AddListener(OnHostButtonClick);
        _joinButton.onClick.AddListener(OnJoinButtonClick);
    }

    private async void OnJoinButtonClick()
    {
        bool succeeded = await _lobbyManager.JoinLobby(_lobbyCodeInput.text);
        if (succeeded)
        {
            Debug.Log($"[Tim] logic after join lobby");
            _lobbyCodeText.text = ApplicationController.Instance.LobbyManager.LobbyCode;
        }
    }

    private async void OnHostButtonClick()
    {
        bool succeeded = await _lobbyManager.CreateLobby();
        if (succeeded)
        {
            Debug.Log($"[Tim] logic after create lobby");
            _lobbyCodeText.text = ApplicationController.Instance.LobbyManager.LobbyCode;
        }
    }

    private void OnClickShopButton()
    {
        // open window shop maybe send data to window or window get data and delete from this script this
    }

    private void OnClickInventoryButton()
    {
        // open window Inventory maybe send data to window or window get data and delete from this script this
    }

    public void LevelUp()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => _xpRect.offsetMax, x => _xpRect.offsetMax = x, new Vector2(0, 0), 2))
            .Append(DOTween.To(() => _xpRect.offsetMax, x => _xpRect.offsetMax = x, new Vector2( -1000, 0), 0.01f))
            .Append(DOTween.To(() => _xpRect.offsetMax, x => _xpRect.offsetMax = x, new Vector2(-700, 0), 1));
    }

    public void Dispose()
    {
        _shopButton.onClick.RemoveListener(OnClickShopButton);
        _inventoryButton.onClick.RemoveListener(OnClickInventoryButton);
        _hostButton.onClick.RemoveListener(OnHostButtonClick);
        _joinButton.onClick.RemoveListener(OnJoinButtonClick);
    }
}
