using Saver;
using TMPro;
using UnityEngine;

public class WelcomeWindow : MonoBehaviour
{
    [SerializeField] private TMP_InputField _input;
    
    public void ShowWindow()
    {
        gameObject.SetActive(true);
    }

    public void SetPlayerName()
    {
        LocalSaver.SetPlayerNickname(_input.text);
        gameObject.SetActive(false);
    }
}