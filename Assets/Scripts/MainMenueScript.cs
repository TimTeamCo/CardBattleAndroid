using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenueScript : MonoBehaviour
{
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _cardButton;
    [SerializeField] private Button _startMatch;

    private void OnEnable()
    {
        _optionsButton.onClick.AddListener(OnClickOptionsButton);
        _shopButton.onClick.AddListener(OnClickShopButton);
        _cardButton.onClick.AddListener(OnClickCardButton);
        _startMatch.onClick.AddListener(OnClickStartMatch);
    }

    private void OnClickOptionsButton()
    {
        Debug.Log("Options Button was Clicked");
    }

    private void OnClickShopButton()
    {
        Debug.Log("ShopButton was Clicked");
    }

    private void OnClickCardButton()
    {
        Debug.Log("CardButton was Clicked");
    }

    private void OnClickStartMatch()
    {
        Debug.Log("StartMatch was Clicked");
    }

    private void OnDestroy()
    {
        _optionsButton.onClick.RemoveListener(OnClickOptionsButton);
        _shopButton.onClick.RemoveListener(OnClickShopButton);
        _cardButton.onClick.RemoveListener(OnClickCardButton);
        _startMatch.onClick.RemoveListener(OnClickStartMatch);
    }
}
