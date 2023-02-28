using UnityEngine;

public class ShopButtonView : AnimatorButtonView
{
    [SerializeField] private CanvasGroup _choosePetCanvas;
    
    private void Start()
    {
        _button.onClick.AddListener(OnClickShopButton);
    }
    
    private void OnClickShopButton()
    {
        AnimateOnClick();
    }

    public void OpenPetWindow()
    {
        _choosePetCanvas.alpha = 1;
        _choosePetCanvas.interactable = true;
        _choosePetCanvas.blocksRaycasts = true;
    }
}