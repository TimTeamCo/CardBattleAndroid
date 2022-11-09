using System; //...
using UnityEngine;

namespace TTBattle.UI
{
    public class MapScript : MonoBehaviour
    {
        //public?
        [SerializeField] public MakeTurn MakeTurn;
        private MapCell _secondRateMapCell;
        private MapCell _newMapCell;
        private MapCell _lastMapCell;
        public ArmyPanel PlayerSelector;
        public ArmyPanel PlayerInferror; //bad naming
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
                    _lastMapCell = MapCell;
                    MakeTurn.MakeButtonEnabled();
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
                        _lastMapCell = MapCell;
                    }
                }
            }
        }
        
        
        private void Start()
        {
            _secondRateMapCell = MapCell;
            MapCell.CellIsSelected();
            InitializePLayersMapCells();
        }

        private void InitializePLayersMapCells()
        {
            PlayerSelector.PlayerMapCell = MapCell;
            PlayerInferror.PlayerMapCell = MapCell;
            GetPlayerInfluence(PlayerSelector.Player);
            GetPlayerInfluence(PlayerInferror.Player);
        }

        private void GetPlayerInfluence(Player player)
        {
            player.UnitsInfluence = MapCell.uintsInfluence;
        }

        //?
        public void SetNewMapCell()
        {
            {
                if (_newMapCell != null && _newMapCell != PlayerInferror.PlayerMapCell)
                {
                    MapCell = NewMapCell;
                    MapCell._isSelected = true;
                    MapCell.SetCellCollorAsPlayers(PlayerSelector);
                    GetPlayerInfluence(PlayerSelector.Player);
                    PlayerSelector.PlayerMapCell = MapCell;
                    _lastMapCell.CellIsLeaved();
                }
                if (_newMapCell != null && _newMapCell == PlayerInferror.PlayerMapCell)
                {
                    MapCell._isAccasible = true;
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
            (PlayerSelector, PlayerInferror) = (PlayerInferror, PlayerSelector); //don't use that like this
            MapCell = PlayerSelector.PlayerMapCell;
            MapCell.CellIsSelected();
        }
    }
}