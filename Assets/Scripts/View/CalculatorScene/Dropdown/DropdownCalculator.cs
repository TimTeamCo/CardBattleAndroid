using Army;
using TTBattle.UI;
using UnityEngine;
using UnityEngine.UI;

public class DropdownCalculator : MonoBehaviour
{
    [HideInInspector] public int Value = 0;
    [SerializeField] public Image DropDownImage;
    [SerializeField] public ArmyPanel ArmyPanel;
    [SerializeField] private GameObject _content;
    [SerializeField] private DropdownItem _content1;
    [SerializeField] private DropdownItem _content2;
    [SerializeField] private DropdownItem _content3;
    [SerializeField] private Text _everything0Text;
    private PlayerArmy _playerArmy;
    
    private void Awake()
    {
        _playerArmy = ArmyPanel.playerDataSo.playerArmy;
    }

    private void OnEnable()
    {
        DropDownImage.sprite = ShowNotNullSprite();
        ShowEverything0Text(DropDownImage.sprite);
    }

    private void UpdateViewDropdownImage()
    {
        foreach (var squad in _playerArmy.Squads)
        {
            switch (squad.SquadUnit.UnitType)
            {
                case UnitType.Warrior :
                    _content1.gameObject.SetActive(squad.Count != 0);
                    break;
                case UnitType.Steamer :
                    _content2.gameObject.SetActive(squad.Count != 0);
                    break;
                case UnitType.Mage :
                    _content3.gameObject.SetActive(squad.Count != 0);
                    break;
                default:
                    break;
            }
        }
    }

    private Sprite ShowNotNullSprite()
    {
        foreach (var squad in _playerArmy.Squads)
        {
            switch (squad.SquadUnit.UnitType)
            {
                case UnitType.Warrior :
                    if (squad.Count != 0)
                    {
                        Value = 0;
                        return _content1._itemSprite;
                    }
                    break;
                case UnitType.Steamer :
                    if (squad.Count != 0)
                    {
                        Value = 1;
                        return _content2._itemSprite;
                    }
                    break;
                case UnitType.Mage :
                    if (squad.Count != 0)
                    {
                        Value = 2;
                        return _content3._itemSprite;
                    }
                    break;
            }
        }

        return null;
    }
    
    public void OnClickDropDown()
    {
        var isShow = _content.activeInHierarchy;
        _content.SetActive(!isShow);
        if (isShow) return;
        UpdateViewDropdownImage();
        ShowEverything0Text(DropDownImage.sprite);
    }

    public void HideContent()
    {
        _content.SetActive(!_content.activeInHierarchy);
    }

    private void ShowEverything0Text(Sprite sprite)
    {
        _everything0Text.gameObject.SetActive(sprite == null);
        DropDownImage.gameObject.SetActive(sprite != null);
    }
}
