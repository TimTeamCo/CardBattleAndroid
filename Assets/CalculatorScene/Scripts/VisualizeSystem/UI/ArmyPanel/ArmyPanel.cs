using System;
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
        [SerializeField] private Image _attackerNotificationImage;
        [SerializeField] private Image _armyPanelImage;
        [SerializeField] private Image _nameImage;
        [SerializeField] private Text _playerName;
        [SerializeField] private SquadCard _warriorCard;
        [SerializeField] private SquadCard _steamerCard;
        [SerializeField] private SquadCard _mageCard;
        [SerializeField] public DropdownCalculator UnitDropdown;
        [SerializeField] public PlayerDataCalculator playerData;
        
        private PlayerSquad _warrior;
        private PlayerSquad _steamer;
        private PlayerSquad _mage;
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
            _warrior = playerData.playerArmy.Squads[0];
            _steamer = playerData.playerArmy.Squads[1];
            _mage = playerData.playerArmy.Squads[2];
            SetTextOfUnitsAmount();
        }

        public void ShowPlayerName()
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
            foreach (var buffZone in playerData.PlayerMapCell.MapZone.buffsZone)
            {
                float unitsInfluence = (float) buffZone.buffValue / 100;
                switch (buffZone.unitType)
                {
                    case UnitType.Warrior:
                        _warriorCard.SetUnitStats(unitsInfluence, _warrior.SquadUnit.Health,
                            _warrior.SquadUnit.Attack);
                        break;
                    case UnitType.Steamer:
                        _steamerCard.SetUnitStats(unitsInfluence, _steamer.SquadUnit.Health,
                            _steamer.SquadUnit.Attack);
                        break;
                    case UnitType.Mage:
                        _mageCard.SetUnitStats(unitsInfluence, _mage.SquadUnit.Health, _mage.SquadUnit.Attack);
                        break;
                }
            }

            _warriorCard.SetBurningDamageText(playerData);
            _steamerCard.SetBurningDamageText(playerData);
            _mageCard.SetBurningDamageText(playerData);
            SetTextOfUnitsAmount();
        }

        private void SetBackgroundColor()
        {
            _armyPanelImage.color = playerData.PlayerColor;
            _nameImage.color = playerData.PlayerColor;
        }

        public void ShowPlayerSelectorNotification(bool value)
        {
            _attackerNotificationImage.gameObject.SetActive(value);
        }
    }
}