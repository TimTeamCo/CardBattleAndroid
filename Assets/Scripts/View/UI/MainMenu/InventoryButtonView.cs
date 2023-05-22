using UnityEngine;

public class InventoryButtonView : AnimatorButtonView
{
    [SerializeField] private GameObject _lobbyWindowObject;
    
    private void OnEnable()
    {
        _button.onClick.AddListener(OnClickInventoryButton);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClickInventoryButton);
    }

    private void OnClickInventoryButton()
    {
        AnimateOnClick();
        OpenLobbyWindow();
    }

    private void OpenLobbyWindow()
    {
        _lobbyWindowObject.SetActive(true);
    }
}
