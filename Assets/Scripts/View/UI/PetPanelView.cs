using Saver;
using TMPro;
using UnityEngine;

public class PetPanelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _petDialog;

    private void Awake()
    {
        var gameManager = ApplicationController.Instance.GameManager;
        gameManager.onApplicationEntry += OnApplicationEntry;
        gameManager.onPressStartButton += OnSearchingBattle;
        gameManager.onExitSearchingButton += OnExitSearchingBattle;
        gameManager.onJoinIntoLobby += OnJoinIntoLobby;
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