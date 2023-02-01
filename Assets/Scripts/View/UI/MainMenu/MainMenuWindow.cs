using DG.Tweening;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private ShopButtonView _shopButtonView;
    [SerializeField] private InventoryButtonView _inventoryButtonView;
    [SerializeField] private RectTransform _xpRect;
    [SerializeField] private RectTransform _dustRect;
    [SerializeField] private TextMeshProUGUI _hamsterDialog;

    private void Start()
    {
        _hamsterDialog.text = $"Hi {AuthenticationService.Instance.PlayerId} !";
    }

    private void OnEnable()
    {
        _shopButton.onClick.AddListener(OnClickShopButton);
        _inventoryButton.onClick.AddListener(OnClickInventoryButton);
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
        _shopButton.onClick.RemoveListener(OnClickShopButton);
        _inventoryButton.onClick.RemoveListener(OnClickInventoryButton);
    }
}
