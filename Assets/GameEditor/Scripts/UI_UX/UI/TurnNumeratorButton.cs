using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class TurnNumeratorButton : MonoBehaviour
    {
        [SerializeField] public Dropdown _PlayerAttackerDropdown;
        [SerializeField] public Dropdown _PlayerDeffenderDropdown;
        [SerializeField] private GameObject Calculator;
        [SerializeField] public GameObject Player1Panel;
        [SerializeField] public GameObject Player2Panel;
        [SerializeField] private GameObject Numerator;
        [SerializeField] private MonoBehaviour _squadAttackScript;
        [SerializeField] private MonoBehaviour _changeArmys;
        //private SquadAttack _squadAttackScript;
        private StartBattle startBattle;
        private ArmyPanelCountUnitsWiew armyPanelCountUnitsWiewPlayer;
        private PlayerTurnNumerator _playerTurnNumerator;
        private int _newTurnsChecker = 0;
        internal int attackNumerator = 1;
        private bool IsChargedMagePlayer1;
        private bool IsChargedMagePlayer2;
        public MapCellScrip MapCellPl1;
        public MapCellScrip MapCellPl2;

        public void MakeTurn()
        {
            MapCellPl1 = Player1Panel.GetComponent<MapScript>().playerMapCell;
            MapCellPl2 = Player2Panel.GetComponent<MapScript>().playerMapCell;
            SetObjectReferences();
            SetNewTurnCount();
            attackNumerator++;
            _squadAttackScript.GetComponent<SquadAttack>().Attack(
                startBattle._player1.GetUnitChoice(_PlayerAttackerDropdown.value),
                startBattle._player2.GetUnitChoice(_PlayerDeffenderDropdown.value), this, MapCellPl1, MapCellPl2);
            SetArmyPanelsValues();
            _changeArmys.GetComponent<ChangeArmys>().DoChangeArmys();

        }

        private void SetArmyPanelsValues()
        {
            armyPanelCountUnitsWiewPlayer = Player1Panel.GetComponent<ArmyPanelCountUnitsWiew>();
            armyPanelCountUnitsWiewPlayer.SetCountUnitsPlayer(startBattle._player1);
            armyPanelCountUnitsWiewPlayer = Player2Panel.GetComponent<ArmyPanelCountUnitsWiew>();
            armyPanelCountUnitsWiewPlayer.SetCountUnitsPlayer(startBattle._player2);
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
            startBattle = Calculator.GetComponent<StartBattle>();
            _playerTurnNumerator = Numerator.GetComponent<PlayerTurnNumerator>();
        }
    }
}