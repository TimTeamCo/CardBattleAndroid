using System;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class ArmyPanel : MonoBehaviour
    {
        [SerializeField] private Image _armyPanelImage;
        [SerializeField] private Image _nameImage;
        [SerializeField] private Sprite _playerChip;
        [SerializeField] private Text _playerName;
        [SerializeField] private SquadCard _warriorCard;
        [SerializeField] private SquadCard _assasinCard;
        [SerializeField] private SquadCard _mageCard;
        [SerializeField] public Dropdown UnitDropdown;
        [SerializeField] public Image UnitDropdownImage;
        [SerializeField] public Image UnitDropdownTemplateImage;
        [SerializeField] public Color PlayerPanelColor;
        public string Name;
        public Player Player = new Player();

        private void Awake()
        {
            CachePlayerValues();
            SetArmyValues();
            SetTextOfCardsAtributes();
            Player.PlayerChip= _playerChip;
            UnitDropdown.gameObject.SetActive(false);
        }

        private void CachePlayerValues()
        {
            Player.PlayerName = Name;
            Player.PlayerColor = PlayerPanelColor;
        }

        public void SetArmyValues()
        {
            SetPlayerName();
            SetTextOfUnitsAmount();
            SetBackgroundColor();
        }

        private void SetPlayerName()
        {
            _playerName.text = Player.PlayerName;
        }
        
        public void SetTextOfUnitsAmount()
        {
            var playerHand = Player.PlayerHand;
            _warriorCard.UnitsNumber.text = playerHand._warriorSquad.Count.ToString();
            _assasinCard.UnitsNumber.text = playerHand._assasinSquad.Count.ToString();
            _mageCard.UnitsNumber.text = playerHand._mageSquad.Count.ToString();
            
        }

        public void SetTextOfCardsAtributes()
        {
            var playerHand = Player.PlayerHand;
            _warriorCard.SetHPCellAtrtibute(Player.UnitsInfluence[0], playerHand._warriorSquad._unit.Health);
            _assasinCard.SetHPCellAtrtibute(Player.UnitsInfluence[1], playerHand._assasinSquad._unit.Health);
            _mageCard.SetHPCellAtrtibute(Player.UnitsInfluence[2], playerHand._mageSquad._unit.Health);
            _warriorCard.SetAPCellAtrtibute(Player.UnitsInfluence[0], playerHand._warriorSquad._unit.Attack);
            _assasinCard.SetAPCellAtrtibute(Player.UnitsInfluence[1], playerHand._assasinSquad._unit.Attack);
            _mageCard.SetAPCellAtrtibute(Player.UnitsInfluence[2], playerHand._mageSquad._unit.Attack);
            _warriorCard.SetBurningDamageText(this);
            _assasinCard.SetBurningDamageText(this);
            _mageCard.SetBurningDamageText(this);
        }

        private void SetBackgroundColor()
        {
            _armyPanelImage.color = Player.PlayerColor;
            _nameImage.color = Player.PlayerColor;
        }
    }
}