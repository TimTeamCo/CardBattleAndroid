using Army;
using Map;
using PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    //add name changer
    public class ArmyPanel : MonoBehaviour
    {
        [SerializeField] private Image _armyPanelImage;
        [SerializeField] private Image _nameImage;
        [SerializeField] private Text _playerName;
        [SerializeField] private SquadCard _warriorCard;
        [SerializeField] private SquadCard _steamerCard;
        [SerializeField] private SquadCard _mageCard;
        [SerializeField] public Dropdown UnitDropdown;
        [SerializeField] public Image UnitDropdownImage;
        [SerializeField] public Image UnitDropdownTemplateImage;
        [SerializeField] public PlayerDataCalculator playerData;

        private PlayerSquad _warrior;
        private PlayerSquad _steamer;
        private PlayerSquad _mage;
        private MapZone _currentMapZone;
        private Color _playerPanelColor;

        private void Awake()
        {
            ShowPlayerName();
            SetArmyValues();
            SetBackgroundColor();
            SetTextOfCardsAttributes();
            UnitDropdown.gameObject.SetActive(false);
        }

        public void SetArmyValues()
        {
            SetTextOfUnitsAmount();
            _warrior = playerData.playerSquad.Squads[0];
            _steamer = playerData.playerSquad.Squads[1];
            _mage = playerData.playerSquad.Squads[2];

            _currentMapZone = playerData.MapZone;
        }

        private void ShowPlayerName()
        {
            _playerName.text = playerData.PlayerName;
        }

        public void SetTextOfUnitsAmount()
        {
            _warriorCard.UnitsNumber.text = _warrior.Count.ToString();
            _steamerCard.UnitsNumber.text = _steamer.Count.ToString();
            _mageCard.UnitsNumber.text = _mage.Count.ToString();
        }

        public void SetTextOfCardsAttributes()
        {
            foreach (var buffZone in _currentMapZone.buffsZone)
            {
                switch (buffZone.unitType)
                {
                    case Army.UnitType.Warrior:
                        _warriorCard.SetUnitStats(buffZone.buffValue, _warrior.SquadUnit.Health,
                            _warrior.SquadUnit.Attack);
                        break;
                    case Army.UnitType.Steamer:
                        _steamerCard.SetUnitStats(buffZone.buffValue, _steamer.SquadUnit.Health,
                            _steamer.SquadUnit.Attack);
                        break;
                    case Army.UnitType.Mage:
                        _mageCard.SetUnitStats(buffZone.buffValue, _mage.SquadUnit.Health, _mage.SquadUnit.Attack);
                        break;
                }
            }

            _warriorCard.SetBurningDamageText(playerData);
            _steamerCard.SetBurningDamageText(playerData);
            _mageCard.SetBurningDamageText(playerData);
        }

        private void SetBackgroundColor()
        {
            _armyPanelImage.color = playerData.PlayerColor;
            _nameImage.color = playerData.PlayerColor;
        }
    }
}