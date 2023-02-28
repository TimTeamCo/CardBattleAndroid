using System.Collections.Generic;
using Saver;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetPanelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _petDialog;
    [SerializeField] private Image _petImage;
    [SerializeField] private List<Sprite> _petSprites;
    private LocalLobby _localLobby;

    private void Awake()
    {
        var gameManager = ApplicationController.Instance.GameManager;
        gameManager.onApplicationEntry += OnApplicationEntry;
        gameManager.onPressStartButton += OnSearchingBattle;
        gameManager.onExitSearchingButton += OnExitSearchingBattle;
        gameManager.onJoinIntoLobby += OnJoinIntoLobby;
        gameManager.onPetChange += OnPetChange;
        _localLobby = ApplicationController.Instance.GameManager.LocalLobby;
        _localLobby.onUserReadyChange += OnUserReadyChange;
        _localLobby.onUserJoined += OnUserJoined;
    }

    private void OnPetChange(PetType pet)
    {
        _petImage.sprite = pet switch
        {
            PetType.Cat1 => _petSprites[0],
            PetType.Cat2 => _petSprites[1],
            PetType.Dog1 => _petSprites[2],
            PetType.Dog2 => _petSprites[3],
            PetType.Dog3 => _petSprites[4],
            PetType.Frog => _petSprites[5],
            _ => _petSprites[0]
        };
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