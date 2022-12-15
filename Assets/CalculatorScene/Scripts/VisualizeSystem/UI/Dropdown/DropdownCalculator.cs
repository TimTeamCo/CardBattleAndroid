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

    public void OnClickDropDown()
    {
        var isShow = _content.activeInHierarchy;
        _content.SetActive(!isShow);
        if (isShow)
        {
            PlayerArmy playerArmy = ArmyPanel.playerData.playerArmy;
            foreach (var squad in playerArmy.Squads)
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
                }
            }
        }
    }

    public void HideContent()
    {
        _content.SetActive(!_content.activeInHierarchy);
    }
}
