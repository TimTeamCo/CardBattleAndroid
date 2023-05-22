using UnityEngine;

public class ShopButtonView : AnimatorButtonView
{
    [SerializeField] private GameObject _petWindow;
    
    private void OnEnable()
    {
        _button.onClick.AddListener(OnClickShopButton);
    }
    
    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClickShopButton);
    }
    
    private void OnClickShopButton()
    {
        AnimateOnClick();
        OpenPetWindow();
    }

    private void OpenPetWindow()
    {
        _petWindow.SetActive(true);
    }
}