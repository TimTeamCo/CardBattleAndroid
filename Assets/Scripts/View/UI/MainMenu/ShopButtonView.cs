using UnityEngine;

public class ShopButtonView : AnimatorButtonView
{
    [SerializeField] private GameObject _petWindow;
    
    private void Start()
    {
        _button.onClick.AddListener(OnClickShopButton);
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