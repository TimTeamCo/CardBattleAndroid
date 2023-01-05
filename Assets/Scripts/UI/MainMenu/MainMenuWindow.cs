using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _startMatch;
    [SerializeField] private StartButtonView _startButtonView;
    [SerializeField] private ShopButtonView _shopButtonView;
    [SerializeField] private InventoryButtonView _inventoryButtonView;
    [SerializeField] private RectTransform _xpRect;
    [SerializeField] private RectTransform _dustRect;

    private bool _searchMatchStart;
    private void OnEnable()
    {
        _shopButton.onClick.AddListener(OnClickShopButton);
        _inventoryButton.onClick.AddListener(OnClickInventoryButton);
        _startMatch.onClick.AddListener(OnClickStartMatch);
    }

    private void OnClickShopButton()
    {
        _shopButtonView.AnimateOnClick();
    }

    private void OnClickInventoryButton()
    {
        _inventoryButtonView.AnimateOnClick();
    }

    private void OnClickStartMatch()
    {
        _startButtonView.AnimateOnClick();
    }

    public void LevelUp()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => _xpRect.offsetMax, x => _xpRect.offsetMax = x, new Vector2(0, 0), 2))
            .Append(DOTween.To(() => _xpRect.offsetMax, x => _xpRect.offsetMax = x, new Vector2( -1000, 0), 0.01f))
            .Append(DOTween.To(() => _xpRect.offsetMax, x => _xpRect.offsetMax = x, new Vector2(-700, 0), 1));
    }
    
    private void OnDestroy()
    {
        _shopButton.onClick.RemoveListener(OnClickShopButton);
        _inventoryButton.onClick.RemoveListener(OnClickInventoryButton);
        _startMatch.onClick.RemoveListener(OnClickStartMatch);
    }
}
