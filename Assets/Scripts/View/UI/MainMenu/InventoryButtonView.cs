public class InventoryButtonView : AnimatorButtonView
{
    private void Start()
    {
        _button.onClick.AddListener(OnClickShopButton);
    }
    
    private void OnClickShopButton()
    {
        AnimateOnClick();
    }
}
