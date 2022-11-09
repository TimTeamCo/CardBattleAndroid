using System; //not use this library
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class MakeTurn : MonoBehaviour
    {
        [SerializeField] private ArmyPanel _army1;
        [SerializeField] private ArmyPanel _army2;
        [SerializeField] private TurnsNumerator _turnsNumerator;
        [SerializeField] private SquadAttack _squadAttack;
        [SerializeField] private MapScript _map;
        [SerializeField] private Button _turnButton;
        [SerializeField] private Image _turnImage;
        [SerializeField] public Color EnabledButtonColor;
        [SerializeField] public Color DisabledButtonColor;
        private int _newTurnsChecker;
        public bool IsAttack;
        
        private void Awake()
        {
            MakeButtonDisabled();
        }

        public void DoMakeTurn()
        {
            SetNewTurnCount();
            if (IsAttack)
            {
                _squadAttack.Attack(_army1, _army2, _turnsNumerator);
                SetTextOfArmyUnitsAmount();
            }
            _map.ChangeMapCells();
            ReplaceArmy.DoReplaceArmys(_army1, _army2);
            MakeButtonDisabled();
            IsAttack = false;
        }

        private void SetTextOfArmyUnitsAmount()
        {
            _army1.SetTextOfUnitsAmount();
            _army2.SetTextOfUnitsAmount();
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
            _map.ChangeMapCells(); //Why one line in method?
        }

        public void MakeButtonEnabled()
        {
            _turnButton.enabled = true;
            _turnImage.color = EnabledButtonColor;
        }

        public void MakeButtonDisabled()
        {
            _turnButton.enabled = false;
            _turnImage.color = DisabledButtonColor;
        }
    }
}

