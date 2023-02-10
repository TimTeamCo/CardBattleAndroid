using Saver;
using TMPro;
using UnityEngine;

public class PetPanelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _petDialog;

    private void Awake()
    {
        ApplicationController.Instance.GameManager.onApplicationEntry += OnApplicationEntry;
    }

    private void OnApplicationEntry()
    {
        _petDialog.text = $"Hi {LocalSaver.GetPlayerNickname()}!";
    }
}