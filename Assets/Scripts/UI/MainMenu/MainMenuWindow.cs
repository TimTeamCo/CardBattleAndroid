using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private ShopButtonView _shopButtonView;
    [SerializeField] private InventoryButtonView _inventoryButtonView;
    [SerializeField] private RectTransform _xpRect;
    [SerializeField] private RectTransform _dustRect;
    private Sequence _optionSequence;

    private bool _searchMatchStart;
    private void OnEnable()
    {
        _optionsButton.onClick.AddListener(OnClickOptionsButton);
        _shopButton.onClick.AddListener(OnClickShopButton);
        _inventoryButton.onClick.AddListener(OnClickInventoryButton);
    }

    private void OnClickOptionsButton()
    {
        if (_optionSequence != null)
        {
            DOTween.Kill(_optionSequence);
        }
        
        _optionSequence = DOTween.Sequence();
        _optionSequence.Append(_optionsButton.transform.DOLocalRotate(
            new Vector3(_optionsButton.transform.localRotation.x, _optionsButton.transform.localRotation.y, -720f), 2,
            RotateMode.FastBeyond360));
    }

    private void OnClickShopButton()
    {
        // open window shop maybe send data to window or window get data and delete from this script this
    }

    private void OnClickInventoryButton()
    {
        // open window Inventory maybe send data to window or window get data and delete from this script this
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
        _optionsButton.onClick.RemoveListener(OnClickOptionsButton);
        _shopButton.onClick.RemoveListener(OnClickShopButton);
        _inventoryButton.onClick.RemoveListener(OnClickInventoryButton);
    }
}
