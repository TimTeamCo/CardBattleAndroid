using System;
using System.Collections.Generic;
using PlayerData;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;
using View.Core;

public class LobbyWindowView : MonoBehaviour, IDisposable
{
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private GameObject _opponentObject;
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _opponentName;
    [SerializeField] private Image _playerPet;
    [SerializeField] private Image _opponentPet;
    [SerializeField] Sprite[] _petIcons;
    LocalLobby _localLobby;
    LocalPlayer _localPlayer;
    LocalPlayer _opponent;
    private LobbyPlayerData _data;

    public void Init()
    {
        _localLobby = ApplicationController.Instance.GameManager.LocalLobby;
        _localLobby.onUserJoined += OnUserJoined;
        _localLobby.onUserLeft += OnUserLeft;
        _localLobby.LocalLobbyState.onChanged += OnLobbyStateChanged;
        LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
    }

    private void OnLobbyUpdated()
    {
        List<LobbyPlayerData> playerDatas = ApplicationController.Instance.GameLobbyManager.GetPlayers();

        for (int i = 0; i < playerDatas.Count; i++)
        {
            if (playerDatas[i].Id == AuthenticationService.Instance.PlayerId)
            {
                SetPlayerData(playerDatas[i]);
            }
            else
            {
                SetOpponentData(playerDatas[i]);
            }
        }
    }

    private void SetPlayerData(LobbyPlayerData playerData)
    {
        _data = playerData;
        _playerObject.SetActive(true);
        _playerName.text = playerData.Nickname;
    }

    private void SetOpponentData(LobbyPlayerData playerData)
    {
        _data = playerData;
        _opponentObject.SetActive(true);
        _opponentName.text = playerData.Nickname;
    }

    private void OnLobbyStateChanged(LobbyState obj)
    {
        if (obj == LobbyState.CountDown)
        {
            Debug.Log("Show Lobby View");
            ShowLobby();
        }
        else
        {
            HideLobby();
        }
    }
    
    private void ShowLobby()
    {
        gameObject.SetActive(true);
    }

    public void HideLobby()
    {
        gameObject.SetActive(false);
    }

    private void OnUserLeft(int obj)
    {
        SyncPlayerUI();
    }

    private void OnUserJoined(LocalPlayer obj)
    {
        Debug.Log($"Lobby Window User Joined {obj.PlayerName.Value}");
        SyncPlayerUI();
    }

    private void SyncPlayerUI()
    {
        var myPlayer = ApplicationController.Instance.GameManager.LocalPlayer;
        ResetUI();
        for (int i = 0; i < _localLobby.PlayerCount; i++)
        {
            var player = _localLobby.GetLocalPlayer(i);
            if (player.ID.Value == myPlayer.ID.Value)
            {
                SetPlayerUser(player);
            }
            else
            {
                SetOpponentUser(player);
            }
        }
    }

    private void SetOpponentUser(LocalPlayer opponent)
    {
        _opponent = opponent;
        Debug.Log($"Set opponent in view \n name - {opponent.PlayerName.Value}\n host {opponent.IsHost.Value}\n pet {opponent.Pet.Value} ");
        SetOpponentPet(opponent.Pet.Value);
        SetOpponentDisplayName(_opponent.PlayerName.Value);
        SubscribeToOpponentUpdates();
    }

    private void SetPlayerUser(LocalPlayer localPlayer)
    {
        _localPlayer = localPlayer;
        Debug.Log($"Set player in view \n name - {localPlayer.PlayerName.Value}\n host {localPlayer.IsHost.Value}\n pet {localPlayer.Pet.Value} ");
        SetPlayerPet(localPlayer.Pet.Value);
        SetPlayerDisplayName(_localPlayer.PlayerName.Value);
        SubscribeToPlayerUpdates();
    }

    private void ResetUI()
    {
        if (_localPlayer == null)
            return;
        SetPlayerDisplayName("");
        SetOpponentDisplayName("");
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
        if (_localPlayer == null)
            return;
        _localPlayer.Pet.onChanged -= SetPlayerPet;
    }

    private void UnsubscribeToOpponentUpdates()
    {
        if (_opponent == null)
            return;
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
            case PetType.Frog:
                return _petIcons[5];
            default:
                return _petIcons[0];
        }
    }

    public void Dispose()
    {
        _localLobby.onUserJoined -= OnUserJoined;
        _localLobby.onUserLeft -= OnUserLeft;
        _localLobby.LocalLobbyState.onChanged -= OnLobbyStateChanged;
        LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
    }
}