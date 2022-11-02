using System;
using UnityEngine;

namespace TTBattle.UI
{
    public class MapScript : MonoBehaviour
    {
        [SerializeField] public TurnNumeratorButton _turnNumeratorButton;
        public ArmyPanel _playerSelector;
        public ArmyPanel _playerSecondRate;
        public MapCell _mapCell;
        private MapCell _secondRateMapCell;
        private MapCell _newMapCell;
        private MapCell _lastMapCell;
        
        public MapCell NewMapCell
        {
            get
            {
                return _newMapCell;
            }
            set
            { 
                if( _newMapCell == null)
                {
                    _newMapCell = value;
                    _lastMapCell = _mapCell;

                }
                if (value.id != _newMapCell.id)
                {
                    _newMapCell = value;
                    _lastMapCell = _mapCell;
                }
            }
        }
        
        
        private void Start()
        {
            _secondRateMapCell = _mapCell;
            _mapCell.CellIsSelected();
            InitializePLayersMapCells();
        }

        private void InitializePLayersMapCells()
        {
            _playerSelector._playerMapCell = _mapCell;
            _playerSecondRate._playerMapCell = _mapCell;
            GetPlayerInfluence(_playerSelector._player);
            GetPlayerInfluence(_playerSecondRate._player);
        }

        private void GetPlayerInfluence(Player player)
        {
            player._unitsInfluence = _mapCell.uintsInfluence;
        }

        public void SetNewMapCell()
        {
            {
                if (_newMapCell != null && _newMapCell != _playerSecondRate._playerMapCell)
                {
                    _mapCell = NewMapCell;
                    _mapCell._isSelected = true;
                    _mapCell.SetCellCollorAsPlayers(_playerSelector);
                    GetPlayerInfluence(_playerSelector._player);
                    _playerSelector._playerMapCell = _mapCell;
                    _lastMapCell.CellIsLeaved();
                }
            }
        }
        
        public void ChangeMapCells()
        {
            SetNewMapCell();
            (_playerSelector, _playerSecondRate) = (_playerSecondRate, _playerSelector);
            _mapCell = _playerSelector._playerMapCell;
            _mapCell.CellIsSelected();
        }


    }
}