using System; //not use this library
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class MakeTurn : MonoBehaviour
    {
        //Why all public?
        [SerializeField] private ArmyPanel _player1Army; //Want call this Army1
        [SerializeField] private ArmyPanel _player2Army; //Army2
        [SerializeField] private TurnsNumerator _turnsNumerator;
        [SerializeField] private SquadAttack _squadAttack;
        [SerializeField] private ReplaceArmys _replaceArmys;
        [SerializeField] private MapScript _map;
        [SerializeField] public Color _enabledButtonColor;
        [SerializeField] public Color _disabledButtonColor;
        public bool _isAttack;
        private int _newTurnsChecker;
        
        private void Awake()
        {
            MakeButtonDisabled();
        }

        public void DoMakeTurn()
        {
            SetNewTurnCount();
            if (_isAttack)
            {
                //We can push only (_player1Army._player, _player2Army._player, _turnsNumerator)
                _squadAttack.Attack(_player1Army._player._playerHand.GetUnitChoice(_player1Army._unitDropdown.value),
                    _player2Army._player._playerHand.GetUnitChoice(_player2Army._unitDropdown.value), _player1Army._player,
                    _player2Army._player, _turnsNumerator);
                SetAmountOfArmysUnits(); // Bad naming for methods
            }
            MapScript();
            _replaceArmys.DoReplaceArmys();
            MakeButtonDisabled();
            _isAttack = false;
        }

        private void SetAmountOfArmysUnits()
        {
            _player1Army.SetAmountOfUnits();
            _player2Army.SetAmountOfUnits();
        }

        private void SetNewTurnCount()
        {
            //hard to realize
            _newTurnsChecker++;
            if (_newTurnsChecker == 2)
            {
                _turnsNumerator.Numerate();
                _newTurnsChecker = 0;
            }
        }

        private void MapScript()
        {
            _map.ChangeMapCells(); //Why one line in method?
        }

        public void MakeButtonEnabled()
        {
            //.....
            GetComponent<Button>().enabled = true;
            GetComponent<Image>().color = _enabledButtonColor;
        }

        public void MakeButtonDisabled()
        {
            //OMG bad :(
            GetComponent<Button>().enabled = false;
            GetComponent<Image>().color = _disabledButtonColor;
        }
    }
}

