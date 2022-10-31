using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class TurnNumeratorButton : MonoBehaviour
    {
        [SerializeField] public Dropdown _playerAttackerDropdown;
        [SerializeField] public Dropdown _playerDeffenderDropdown;
        [SerializeField] private GameObject _calculator;
        [SerializeField] public GameObject _player1Panel;
        [SerializeField] public GameObject _player2Panel;
        [SerializeField] private GameObject _numerator;
        [SerializeField] private MonoBehaviour _squadAttackScript;
        [SerializeField] private MonoBehaviour _changeArmys;
        //private SquadAttack _squadAttackScript;
        private StartBattle _startBattle;
        private ArmyPanelCountUnitsWiew _armyPanelCountUnitsWiewPlayer;
        private PlayerTurnNumerator _playerTurnNumerator;
        private int _newTurnsChecker = 0;
        internal int _attackNumerator = 1;
        // private Player _player1;
        // private Player _player2;
        public void MakeTurn()
        {
            //_player1 = _player1Panel.GetComponent<Player>();
            //_mapCellPl1 = _player1Panel.GetComponent<MapScript>().playerMapCell;
            //_mapCellPl2 = _player2Panel.GetComponent<MapScript>().playerMapCell;
            SetObjectReferences();
            SetNewTurnCount();
            _attackNumerator++;
            _squadAttackScript.GetComponent<SquadAttack>().Attack(
                _startBattle._player1._playerHand.GetUnitChoice(_playerAttackerDropdown.value),
                _startBattle._player2._playerHand.GetUnitChoice(_playerDeffenderDropdown.value), this, _player1Panel.GetComponent<Player>(), _player2Panel.GetComponent<Player>());
            SetArmyPanelsValues();
            _changeArmys.GetComponent<ChangePlayers>().DoChangeArmys();

        }

        private void SetArmyPanelsValues()
        {
            _armyPanelCountUnitsWiewPlayer = _player1Panel.GetComponent<ArmyPanelCountUnitsWiew>();
            _armyPanelCountUnitsWiewPlayer.SetCountUnitsPlayer(_startBattle._player1._playerHand);
            _armyPanelCountUnitsWiewPlayer = _player2Panel.GetComponent<ArmyPanelCountUnitsWiew>();
            _armyPanelCountUnitsWiewPlayer.SetCountUnitsPlayer(_startBattle._player2._playerHand);
        }

        private void SetNewTurnCount()
        {
            _newTurnsChecker++;
            if (_newTurnsChecker == 2)
            {
                _playerTurnNumerator.Numerate();
                _newTurnsChecker = 0;
            }
        }

        private void SetObjectReferences()
        {
            _startBattle = _calculator.GetComponent<StartBattle>();
            _playerTurnNumerator = _numerator.GetComponent<PlayerTurnNumerator>();
        }
    }
}