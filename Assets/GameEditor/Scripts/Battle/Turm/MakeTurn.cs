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
        [SerializeField] private Image _attackImage;
        [SerializeField] public Color EnabledButtonColor;
        [SerializeField] public Color DisabledButtonColor;
        private int _newTurnsChecker;
        public bool IsAttack;
        
        private void Awake()
        {
            MakeTurnButtonDisabled();
            _attackImage.enabled = false;
        }

        public void DoMakeTurn()
        {
            SetNewTurnCount();
            if (IsAttack)
            {
                Attack();
            }
            ReplaceArmy.Execute(_army1, _army2);
            _map.SetPlayersMapCells();
            _army1.SetTextOfCellAtributes();
            _army2.SetTextOfCellAtributes();
            EndOfTurn();
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

        public void MakeTurnButtonEnabled()
        {
            _turnButton.enabled = true;
            _turnImage.color = EnabledButtonColor;
        }

        public void MakeTurnButtonDisabled()
        {
            _turnButton.enabled = false;
            _turnImage.color = DisabledButtonColor;
        }

        private void Attack()
        {
            _squadAttack.Attack(_army1, _army2, _turnsNumerator);
        }

        public void ExecuteWithAttack()
        {
            IsAttack = true;
            MakeTurnButtonEnabled();
            _attackImage.enabled = true;
        }

        private void EndOfTurn()
        {
            MakeTurnButtonDisabled();
            _attackImage.enabled = false;
            IsAttack = false;
        }
    }
}

