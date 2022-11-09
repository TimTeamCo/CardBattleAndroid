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
        [SerializeField] private Text _warriorNumber;
        [SerializeField] private Text _assasinNumber;
        [SerializeField] private Text _mageNumber;
        [SerializeField] public Dropdown UnitDropdown;
        [SerializeField] public Color PlayerPanelColor;
        [NonSerialized] public Color PlayerMapCellColor;
        public string Name;
        public Player Player = new Player();
        public MapCell PlayerMapCell;

        private void Awake()
        {
            CachePlayerValues();
            SetArmyValues();
            PlayerMapCellColor = PlayerPanelColor;
            
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
            _warriorNumber.text = playerHand._warriorSquad.Count.ToString();
            _assasinNumber.text = playerHand._assasinSquad.Count.ToString();
            _mageNumber.text = playerHand._mageSquad.Count.ToString();
        }

        private void SetBackgroundColor()
        {
            _armyPanelImage.color = Player.PlayerColor;
            _nameImage.color = Player.PlayerColor;
        }
    }
}