using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class ArmyPanel : MonoBehaviour
    {
        public string _name;
        [SerializeField] public Dropdown _unitDropdown;
        [SerializeField] private Text _playerName;
        [SerializeField] private Text _warriorNumber;
        [SerializeField] private Text _assasinNumber;
        [SerializeField] private Text _mageNumber;
        [SerializeField] private Color _playerPanelColor;
        public Player _player;

        private void Start()
        {
            _player = new Player();
            SetArmysValues();
        }

        public void SetArmysValues()
        {
            SetPlayerName();
            SetAmountOfUnits();
        }

        public void SetPlayerName()
        {
            _playerName.text = _name;
        }

        public void SetAmountOfUnits()
        {
            _warriorNumber.text = _player._playerHand._warriorSquad.Count.ToString();
            _assasinNumber.text = _player._playerHand._assasinSquad.Count.ToString();
            _mageNumber.text = _player._playerHand._mageSquad.Count.ToString();
        }
    }
}