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
    [SerializeField] private Animator _animatorStartButton;

    private bool _searchMatchStart;
    private void OnEnable()
    {
        _optionsButton.onClick.AddListener(OnClickOptionsButton);
        _shopButton.onClick.AddListener(OnClickShopButton);
        _cardButton.onClick.AddListener(OnClickCardButton);
        _startMatch.onClick.AddListener(()=> StartCoroutine(OnClickStartMatch()));
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

    // private void OnClickStartMatch()
    // {
    //     Debug.Log("StartMatch was Clicked");
    //     _searchMatchStart = !_searchMatchStart;
    //     string stateName = _searchMatchStart ? "Selected" : "Normal";
    //     _animatorStartButton.Play("Pressed");
    //     _animatorStartButton.Play(stateName);
    // }
    
    private IEnumerator OnClickStartMatch()
    {
        Debug.Log("StartMatch was Clicked");
        _searchMatchStart = !_searchMatchStart;
        string stateName = _searchMatchStart ? "Selected" : "Normal";
        _animatorStartButton.Play("Pressed");
        yield return new WaitForSeconds(0.7f);
        _animatorStartButton.Play(stateName);
    }

    private void OnDestroy()
    {
        _optionsButton.onClick.RemoveListener(OnClickOptionsButton);
        _shopButton.onClick.RemoveListener(OnClickShopButton);
        _cardButton.onClick.RemoveListener(OnClickCardButton);
        _startMatch.onClick.RemoveListener(()=> StopCoroutine(OnClickStartMatch()));
    }
}
