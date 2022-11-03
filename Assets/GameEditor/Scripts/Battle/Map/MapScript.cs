using System; //...
using UnityEngine;

namespace TTBattle.UI
{
    public class MapScript : MonoBehaviour
    {
        //public?
        [SerializeField] public MakeTurn _makeTurn;
        public ArmyPanel _playerSelector;
        public ArmyPanel _playerSecondRate; //bad naming
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
                    _makeTurn.MakeButtonEnabled();
                }
                if (value.id != _newMapCell.id)
                {
                    if (!_newMapCell._isSelected)
                    {                    
                        _newMapCell.SetImageColorToUsual();
                    }
                    else
                    {
                        _newMapCell = value;
                        _lastMapCell = _mapCell;
                    }
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

        //?
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
                if (_newMapCell != null && _newMapCell == _playerSecondRate._playerMapCell)
                {
                    _mapCell._isAccasible = true;
                    foreach (MapCell mapCell in _mapCell.NextCell)
                    {
                        mapCell._isAccasible = false;
                    }
                }
            }
        }
        
        public void ChangeMapCells()
        {
            SetNewMapCell();
            (_playerSelector, _playerSecondRate) = (_playerSecondRate, _playerSelector); //don't use that like this
            _mapCell = _playerSelector._playerMapCell;
            _mapCell.CellIsSelected();
        }
    }
}