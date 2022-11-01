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
        [SerializeField] public Color _playerPanelColor;
        [SerializeField] private GameObject _playerPanel;
        [SerializeField] private GameObject _playerNameBg;
        [SerializeField] private Sprite _playerChip;
        public Player _player;

        private void Awake()
        {
            _player = new Player();
            SetArmysValues();
        }

        public void SetArmysValues()
        {
            SetPlayerName();
            SetAmountOfUnits();
            SetBGsColor();
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

        private void SetBGsColor()
        {
            _playerPanel.GetComponent<Image>().color = _playerPanelColor;
            _playerNameBg.GetComponent<Image>().color = _playerPanelColor;
        }
    }
}