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
            _attackImage.gameObject.SetActive(true);
            _attackImage.enabled = false;
        }

        public void DoMakeTurn()
        {
            SetNewTurnCount();
            Attack();
            MapScripts();
            SetBurningDamageToPlayers();
            SetTextOfCellAtributesToArmys();
            EndOfTurn();
        }

        private void MapScripts()
        {
            _map.SetPlayersMapCells();
            if (_newTurnsChecker != 0 || _turnsNumerator.TurnsCount % 5 != 0) return;
            _map.SetBurningZones(_turnsNumerator.TurnsCount);
        }

        private void SetBurningDamageToPlayers()
        {
            if (_newTurnsChecker == 0)
            {
                _army1.playerData.playerArmy.AddBurningDamageToUnits(_army1.playerData.MapZone.burnFactor);
                _army2.playerData.playerArmy.AddBurningDamageToUnits(_army2.playerData.MapZone.burnFactor);
            }
        }
        
        private void SetTextOfCellAtributesToArmys()
        {
            _army1.SetTextOfCardsAttributes();
            _army2.SetTextOfCardsAttributes();
        }
        
        private void SetNewTurnCount()
        {
            _newTurnsChecker++;
            if (_newTurnsChecker == 2)
            {
                _turnsNumerator.Numerate();
                _newTurnsChecker = 0;
            }
            _turnsNumerator.MoveCount++;
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
            if (IsAttack == false)
            {
                return;
            }

            _squadAttack.Attack(_map.PlayerSelector, _map.PlayerInferior, _turnsNumerator);
            _army1.UnitDropdown.gameObject.SetActive(false);
            _army2.UnitDropdown.gameObject.SetActive(false);
            _army1.SetTextOfUnitsAmount();
            _army2.SetTextOfUnitsAmount();
        }

        public void ExecuteWithAttack()
        {
            IsAttack = true;
            MakeTurnButtonEnabled();
            _attackImage.enabled = true;
            _army1.UnitDropdown.gameObject.SetActive(true);
            _army2.UnitDropdown.gameObject.SetActive(true);
        }

        private void EndOfTurn()
        {
            MakeTurnButtonDisabled();
            _attackImage.enabled = false;
            IsAttack = false;
            _map.NextCellInformer.gameObject.SetActive(true);
            _map.NextCellInformer.IsNotCelected();
        }
    }
}