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
                    _newMapCell = value;
                    MakeTurn.MakeTurnButtonEnabled();
                }
                if (value.id != _newMapCell.id)
                {
                    if (!_newMapCell._isTaken)
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
            MapCell.CellIsTaken();
        }

        private void InitializePLayersMapCells()
        {
            PlayerSelector.Player.PlayerMapCell = MapCell;
            PlayerInferior.Player.PlayerMapCell = MapCell;
            PlayerSelector.Player.GetUnitsInfluence();
            PlayerInferior.Player.GetUnitsInfluence();
            PlayerSelector.Player.SetPlayerChipToCell();
        }
        
        private void SetPlayerInferiorMapCell()
        {
            {
                if (_newMapCell.id != MapCell.id)
                {
                    _lastMapCell = MapCell;
                    MapCell = NewMapCell;
                    MapCell._isTaken = true;
                    MapCell.SetCellCollorAsPlayers(PlayerInferior.Player);
                    PlayerInferior.Player.PlayerMapCell = MapCell;
                    PlayerInferior.Player.GetUnitsInfluence();
                    PlayerInferior.Player.SetPlayerChipToCell();
                    _lastMapCell.CellIsLeaved();
                    _newMapCell = null;
                }
                else
                {
                    foreach (MapCell mapCell in MapCell.NextCell)
                    {
                        mapCell._isAccasible = false;
                    }
                    _newMapCell = null;
                }
            }
        }

        private void SetPlayerSelectorMapCell()
        {
            MapCell = PlayerSelector.Player.PlayerMapCell;
            MapCell.CellIsTaken();
            PlayerSelector.Player.PlayerMapCell = MapCell;
            PlayerSelector.Player.SetPlayerChipToCell();
        }
        
        public void SetPlayersMapCells()
        {
            SetPlayerInferiorMapCell();
            SetPlayerSelectorMapCell();
        }
    }
}