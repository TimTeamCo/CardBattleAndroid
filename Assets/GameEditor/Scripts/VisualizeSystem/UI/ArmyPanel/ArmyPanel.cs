using System;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class ArmyPanel : MonoBehaviour
    {
        //TODO: Tim need wright sort modifiers and naming in our dev team 
        public string _name;
        [SerializeField] public Dropdown _unitDropdown;
        [SerializeField] private Text _playerName;
        [SerializeField] private Text _warriorNumber;
        [SerializeField] private Text _assasinNumber;
        [SerializeField] private Text _mageNumber;
        [SerializeField] public Color _playerPanelColor;
        [SerializeField] private GameObject _playerPanel;
        [SerializeField] private GameObject _playerNameBg;
        [SerializeField] private Sprite _playerChip; // no use in code, need?
        public Player _player;
        public MapCell _playerMapCell;
        [NonSerialized] public Color _playerMapCellColor; 

        private void Awake()
        {
            _player = new Player(); //why dont call new player in init?
            SetArmysValues();
            _playerMapCellColor = _playerPanelColor;
        }

        //Army
        public void SetArmysValues()
        {
            SetPlayerName();
            SetAmountOfUnits();
            SetBGsColor();
        }

        //why public?
        public void SetPlayerName()
        {
            _playerName.text = _name;
        }
        
        /// <summary>
        /// var playerHand = _player._playerHand;
        /// </summary>
        public void SetAmountOfUnits()
        {
            _warriorNumber.text = _player._playerHand._warriorSquad.Count.ToString();
            _assasinNumber.text = _player._playerHand._assasinSquad.Count.ToString();
            _mageNumber.text = _player._playerHand._mageSquad.Count.ToString();
        }

        private void SetBGsColor()
        {
            //cache GetComponent
            _playerPanel.GetComponent<Image>().color = _playerPanelColor;
            _playerNameBg.GetComponent<Image>().color = _playerPanelColor;
        }
    }
}