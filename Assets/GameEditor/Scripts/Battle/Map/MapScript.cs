using System;
using UnityEngine;

namespace TTBattle.UI
{
    public class MapScript : MonoBehaviour
    {
        public ArmyPanel _playerSelector;
        public ArmyPanel _playerSecondRate;
        public MapCell _mapCell;
        public MapCell _secondMapCell;
        [SerializeField] public TurnNumeratorButton _turnNumeratorButton;
        public MapCell _newMapCell;

        private void Start()
        {
            _playerSelector._player._unitsInfluence = _mapCell.uintsInfluence;
            _playerSecondRate._player._unitsInfluence = _mapCell.uintsInfluence;
            _mapCell.CellIsSelected();
            _secondMapCell = _mapCell;
        }

        private void GetPlayerInfluence(MapCell mapCell)
        {
            if (mapCell.ToString() == "_secondMapCell")
            {_playerSecondRate._player._unitsInfluence = mapCell.uintsInfluence;}
            else
            {_playerSelector._player._unitsInfluence = mapCell.uintsInfluence;}
        }

        public void SetNewMapCell(MapCell mapCell)
        {
            if (mapCell.id != _mapCell.id)
            {
                _secondMapCell._isSelected = true;
                _secondMapCell = mapCell;
                GetPlayerInfluence(_secondMapCell);
            }
        }

        public void ChangeMapCells()
        {
            (_playerSelector, _playerSecondRate) = (_playerSecondRate, _playerSelector);
            // if (_newMapCell == _mapCell)
            // {
            //     _secondMapCell.CellIsSelected();
            //
            // } 
            // if (_newMapCell == _secondMapCell)
            // {
            //     _mapCell.CellIsSelected();
            // }
            (_mapCell, _secondMapCell) = (_secondMapCell, _mapCell);
            
        }


        public void GetNewMapCell(MapCell newMapCell)
        {
            if (_newMapCell != null)
            {
                if (newMapCell._isSelected == true)
                {
                }
                else if (_newMapCell.id != newMapCell.id)
                {
                    _newMapCell.LeaveCell();
                    _newMapCell = newMapCell;
                }
            }
            else
            {
                _newMapCell = newMapCell;
            }
        }
    }
}