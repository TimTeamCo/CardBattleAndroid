using System;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class ArmyPanel : MonoBehaviour
    {
        public string _name;
        [SerializeField] private Image _playerPanelImage;
        [SerializeField] public Dropdown _unitDropdown;
        [SerializeField] private Text _playerName;
        [SerializeField] private Text _warriorNumber;
        [SerializeField] private Text _assasinNumber;
        [SerializeField] private Text _mageNumber;
        [SerializeField] public Color _playerPanelColor;
        [SerializeField] private GameObject _playerPanel;
        [SerializeField] private GameObject _playerNameBg;
        [SerializeField] private Sprite _playerChip;
        public Player _player = new Player();
        public MapCell _playerMapCell;
        [NonSerialized] public Color _playerMapCellColor;

        private void Awake()
        {
            SetArmyValues();
            _playerMapCellColor = _playerPanelColor;
            
        }

        public void SetArmyValues()
        {
            SetPlayerName();
            SetTextOfUnitsAmount();
            SetBackgroundColor();
        }

        private void SetPlayerName()
        {
            _playerName.text = _name;
        }
        
        /// <summary>
        /// var playerHand = _player._playerHand;
        /// </summary>
        public void SetTextOfUnitsAmount()
        {
            var playerHand = _player._playerHand;
            _warriorNumber.text = playerHand._warriorSquad.Count.ToString();
            _assasinNumber.text = playerHand._assasinSquad.Count.ToString();
            _mageNumber.text = playerHand._mageSquad.Count.ToString();
        }

        private void SetBackgroundColor()
        {
            //cache GetComponent
            _playerPanelImage.color = _playerPanelColor;
            _playerNameBg.GetComponent<Image>().color = _playerPanelColor;
        }
    }
}