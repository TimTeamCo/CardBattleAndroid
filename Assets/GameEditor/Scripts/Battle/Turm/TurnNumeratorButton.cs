using UnityEngine;

namespace TTBattle.UI
{
    public class TurnNumeratorButton : MonoBehaviour
    {
        [SerializeField] private ArmyPanel _player1Army;
        [SerializeField] private ArmyPanel _player2Army;
        [SerializeField] private TurnsNumerator _turnsNumerator;
        [SerializeField] private SquadAttack _squadAttack;
        [SerializeField] private ReplaceArmys _replaceArmys;
        [SerializeField] private MapScript _map;
        private int _newTurnsChecker;

        public void MakeTurn()
        {
            SetNewTurnCount();
            MapScript();
            _squadAttack.Attack(_player1Army._player._playerHand.GetUnitChoice(_player1Army._unitDropdown.value),
                _player2Army._player._playerHand.GetUnitChoice(_player2Army._unitDropdown.value), _player1Army._player,
                _player2Army._player, _turnsNumerator);
            SetAmountOfArmysUnits();
            _replaceArmys.DoReplaceArmys();
        }

        private void SetAmountOfArmysUnits()
        {
            _player1Army.SetAmountOfUnits();
            _player2Army.SetAmountOfUnits();
        }

        private void SetNewTurnCount()
        {
            _newTurnsChecker++;
            if (_newTurnsChecker == 2)
            {
                _turnsNumerator.Numerate();
                _newTurnsChecker = 0;
            }
        }

        private void MapScript()
        {
            _map.ChangeMapCells();
        }
    }
}

