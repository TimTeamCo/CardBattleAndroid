using System; //...
using UnityEngine;

namespace TTBattle.UI
{
    public class MapScript : MonoBehaviour
    {
        [SerializeField] public MakeTurn MakeTurn;
        private MapCell _secondRateMapCell;
        private MapCell _newMapCell;
        private MapCell _lastMapCell;
        public ArmyPanel PlayerSelector;
        public ArmyPanel PlayerInferior;
        public MapCell MapCell;
        
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
                    _lastMapCell = MapCell;
                    _newMapCell = value;
                    MakeTurn.MakeButtonEnabled();
                }
                if (value.id != _newMapCell.id)
                {
                    if (!_newMapCell._isSelected)
                    { 
                        _newMapCell.SetImageColorToUsual();
                        _newMapCell = value;
                    }
                    else
                    {
                        _newMapCell.SetCellCollorAsPlayers(PlayerSelector.Player);
                        _newMapCell = value;
                    }
                }
            }
        }
        
        public void Start()
        {
            InitializePLayersMapCells();
            MapCell.CellIsSelected();
        }

        private void InitializePLayersMapCells()
        {
            PlayerSelector.Player.PlayerMapCell = MapCell;
            PlayerInferior.Player.PlayerMapCell = MapCell;
            PlayerSelector.Player.GetUnitsInfluence();
            PlayerInferior.Player.GetUnitsInfluence();
        }

        //?
        public void SetNewMapCell()
        {
            {
                if (_newMapCell != MapCell)
                {
                    MapCell = NewMapCell;
                    MapCell._isSelected = true;
                    MapCell.SetCellCollorAsPlayers(PlayerSelector.Player);
                    PlayerSelector.Player.PlayerMapCell = MapCell;
                    PlayerSelector.Player.GetUnitsInfluence();
                    _lastMapCell.CellIsLeaved();
                }
                else
                {
                    foreach (MapCell mapCell in MapCell.NextCell)
                    {
                        mapCell._isAccasible = false;
                    }
                }
            }
        }
        
        public void ChangeMapCells()
        {
            SetNewMapCell();
            ChangePlayers();
            MapCell = PlayerSelector.Player.PlayerMapCell;
            MapCell.CellIsSelected();
        }

        private void ChangePlayers()
        {
            var player = PlayerSelector;
            PlayerInferior = PlayerSelector;
            PlayerSelector = player;
        }
    }
}