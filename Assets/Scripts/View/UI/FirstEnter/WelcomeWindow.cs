using System;
using Saver;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeWindow : MonoBehaviour
{
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private TextMeshProUGUI _warning;
    [SerializeField] private Button _setButton;
    
    public void ShowWindow()
    {
        _input.onValidateInput += OnValidateInput;
        gameObject.SetActive(true);
    }

    private char OnValidateInput(string text, int charindex, char addedchar)
    {
        if (char.IsLetterOrDigit(addedchar))
        {
            _warning.text = "";
            return addedchar;
        }
        _warning.text = $"You can enter only letters and digits\n";
        return '*';
    }

    public void SetPlayerName()
    {
        if (string.IsNullOrEmpty(_input.text))
        {
            _warning.text = $"We can`t set empty nickname\n";
            return;
        }
        
        if (_input.text.Contains('*'))
        {
            _warning.text = $"You can enter only letters and digits\n";
            return;
        }
        
        LocalSaver.SetPlayerNickname(_input.text);
        gameObject.SetActive(false);
        ApplicationController.Instance.GameManager.CreateLocalData();
    }

    private void OnDisable()
    {
        _input.onValidateInput -= OnValidateInput;
    }
}