using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class MakeTurn : MonoBehaviour
    {
        [SerializeField] private ArmyPanelManager armyPanelManager;
        [SerializeField] private TurnsNumerator _turnsNumerator;
        [SerializeField] private NextCellInformer _nextCellInformer;
        [SerializeField] private SquadAttack _squadAttack;
        [SerializeField] private MapScript _map;
        [SerializeField] private Button _turnButton;
        [SerializeField] private Image _turnImage;
        [SerializeField] private Image _attackImage;
        [SerializeField] public Color EnabledButtonColor;
        [SerializeField] public Color DisabledButtonColor;
        private ArmyPanel _armySelector;
        private ArmyPanel _armyInferior;
        private int _newTurnsChecker;
        public bool IsAttack;
        
        private void Awake()
        {
            SetArmys();
            MakeTurnButtonDisabled();
            ShowPlayerSelectorNotification();
            _attackImage.gameObject.SetActive(true);
            _attackImage.enabled = false;
        }

        public void DoMakeTurn()
        {
            SetNewTurnCount();
            Attack();
            ChangePlayersRoles();
            ShowPlayerSelectorNotification();
            MapScripts();
            SetBurningDamageToPlayers();
            SetTextOfCellAtributesToArmys();
            EndOfTurn();
        }

        private void ShowPlayerSelectorNotification()
        {
            _armySelector.ShowPlayerSelectorNotification(true);
            _armyInferior.ShowPlayerSelectorNotification(false);
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
                _armySelector.playerData.playerArmy.AddBurningDamageToUnits(_armySelector.playerData.MapZone.burnFactor);
                _armyInferior.playerData.playerArmy.AddBurningDamageToUnits(_armyInferior.playerData.MapZone.burnFactor);
            }
        }
        
        private void SetTextOfCellAtributesToArmys()
        {
            _armySelector.SetTextOfCardsAttributes();
            _armyInferior.SetTextOfCardsAttributes();
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

        private void MakeTurnButtonDisabled()
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

            _squadAttack.Attack(_armySelector, _armyInferior, _turnsNumerator);
            _armySelector.UnitDropdown.gameObject.SetActive(false);
            _armyInferior.UnitDropdown.gameObject.SetActive(false);
            _armySelector.SetTextOfUnitsAmount();
            _armyInferior.SetTextOfUnitsAmount();
            _attackImage.enabled = false;
            IsAttack = false;
        }

        public void ExecuteWithAttack()
        {
            IsAttack = true;
            _attackImage.enabled = true;
            _nextCellInformer.gameObject.SetActive(false);
            _armySelector.UnitDropdown.gameObject.SetActive(true);
            _armyInferior.UnitDropdown.gameObject.SetActive(true);
        }

        private void EndOfTurn()
        {
            MakeTurnButtonDisabled();
            _map.NextCellInformer.gameObject.SetActive(true);
            _map.NextCellInformer.IsNotCelected();
        }

        private void ChangePlayersRoles()
        {
            armyPanelManager.ChangePlayersRoles();
            SetArmys();
        }

        public void SetArmys()
        {
            _armySelector = armyPanelManager.PlayerSelector;
            _armyInferior = armyPanelManager.PlayerInferior;
        }

        public void UndoAttack()
        {
            IsAttack = false;
            _attackImage.enabled = false;
            _nextCellInformer.gameObject.SetActive(true);
            _armySelector.UnitDropdown.gameObject.SetActive(false);
            _armyInferior.UnitDropdown.gameObject.SetActive(false); 
        }
    }
}