using Saver;
using TMPro;
using UnityEngine;

public class PetPanelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _petDialog;
    private LocalLobby _localLobby;

    private void Awake()
    {
        var gameManager = ApplicationController.Instance.GameManager;
        gameManager.onApplicationEntry += OnApplicationEntry;
        gameManager.onPressStartButton += OnSearchingBattle;
        gameManager.onExitSearchingButton += OnExitSearchingBattle;
        gameManager.onJoinIntoLobby += OnJoinIntoLobby;
        _localLobby = ApplicationController.Instance.GameManager.LocalLobby;
        _localLobby.onUserReadyChange += OnUserReadyChange;
        _localLobby.onUserJoined += OnUserJoined;
    }

    private void OnUserReadyChange(int obj)
    {
        if (obj == 1)
        {
            _petDialog.text = $"Cool! Wait second ready status";
        }
    }

    private void OnUserJoined(LocalPlayer localPlayer)
    {
        if (_localLobby.PlayerCount != 2) return;
        _petDialog.text = $"Ready?\nPush Start";
    }

    private void OnJoinIntoLobby()
    {
        _petDialog.text = $"Wait until I find your opponent...";
    }

    private void OnApplicationEntry()
    {
        _petDialog.text = $"Hi {LocalSaver.GetPlayerNickname()}!";
    }
    
    private void OnSearchingBattle()
    {
        _petDialog.text = $"Searching battle...";
    }
    
    private void OnExitSearchingBattle()
    {
        _petDialog.text = $"I love you!";
    }
}