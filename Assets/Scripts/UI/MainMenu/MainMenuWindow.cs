using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _startMatch;
    [SerializeField] private StartButtonView _startButtonView;
    [SerializeField] private ShopButtonView _shopButtonView;
    [SerializeField] private InventoryButtonView _inventoryButtonView;
    private Sequence _optionSequence;

    private bool _searchMatchStart;
    private void OnEnable()
    {
        _optionsButton.onClick.AddListener(OnClickOptionsButton);
        _shopButton.onClick.AddListener(OnClickShopButton);
        _inventoryButton.onClick.AddListener(OnClickInventoryButton);
        _startMatch.onClick.AddListener(OnClickStartMatch);
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

    private void OnDestroy()
    {
        _optionsButton.onClick.RemoveListener(OnClickOptionsButton);
        _shopButton.onClick.RemoveListener(OnClickShopButton);
        _inventoryButton.onClick.RemoveListener(OnClickInventoryButton);
        _startMatch.onClick.RemoveListener(OnClickStartMatch);
    }
}
