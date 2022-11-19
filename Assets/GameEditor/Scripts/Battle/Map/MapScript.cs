using System.Collections.Generic;
using UnityEngine;

namespace TTBattle.UI
{
    public class MapScript : MonoBehaviour
    {
        [SerializeField] private List<MapCell> _fistBurningZone = new List<MapCell>();
        [SerializeField] private List<MapCell> _secondBurningZone = new List<MapCell>();
        [SerializeField] private List<MapCell> _thirdBurningZone = new List<MapCell>();
        [SerializeField] public MakeTurn MakeTurn;
        private MapCell _secondRateMapCell;
        private MapCell _newMapCell;
        private MapCell _lastMapCell;
        public Sprite FireStage1;
        public Sprite FireStage2;
        public Sprite FireStage3;
        public ArmyPanel PlayerSelector;
        public ArmyPanel PlayerInferior;
        public MapCell MapCell;
        public NextCellInformer NextCellInformer;

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
                    if (!_newMapCell.IsTaken)
                    { 
                        _newMapCell.SetImageColorToUsual();
                        _newMapCell = value;
                    }
                    else
                    {
                        PlayerSelector.Player.PlayerMapCell.SetCellCollorAsPlayers(PlayerSelector.Player);
                        _newMapCell = value;
                    }
                }
            }
        }
        
        public void Awake()
        {
            InitializePLayersMapCells();
        }

        private void Start()
        { 
            MapCell.CellIsTaken();
        }

        private void InitializePLayersMapCells()
        {
            PlayerSelector.Player.PlayerMapCell = MapCell;
            PlayerInferior.Player.PlayerMapCell = MapCell;
            PlayerSelector.Player.GetUnitsInfluence();
            PlayerInferior.Player.GetUnitsInfluence();
        }
        
        private void SetPlayerInferiorMapCell()
        {
            {
                if (_newMapCell.id != MapCell.id)
                {
                    _lastMapCell = MapCell;
                    MapCell = NewMapCell;
                    MapCell.IsTaken = true;
                    MapCell.SetCellCollorAsPlayers(PlayerInferior.Player);
                    PlayerInferior.Player.PlayerMapCell = MapCell;
                    PlayerInferior.Player.GetUnitsInfluence();
                    MapCell.SetChipSprite(PlayerInferior.Player.PlayerChip);
                    _lastMapCell.CellIsLeaved();
                    _newMapCell = null;
                }
                else
                {
                    foreach (MapCell mapCell in MapCell.NextCell)
                    {
                        mapCell.IsAccasible = false;
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
        }
        
        public void SetPlayersMapCells()
        {
            SetPlayerInferiorMapCell();
            SetPlayerSelectorMapCell();
        }

        public void SetBurningZones(int nomber)
        {
            if (nomber == 1)
            {
                foreach (MapCell mapCell in _fistBurningZone)
                {
                    mapCell.BurningDamage += 3;
                    if (PlayerSelector.Player.PlayerMapCell.id != mapCell.id && PlayerInferior.Player.PlayerMapCell.id != mapCell.id)
                    {
                        mapCell.SetAlphaChipSprite(1f);
                        if (mapCell.BurningDamage==3)
                        {
                            mapCell.ChipImage.sprite = FireStage1;
                        }
                        if (mapCell.BurningDamage==6)
                        {
                            mapCell.ChipImage.sprite = FireStage2;
                        }
                        if (mapCell.BurningDamage==9)
                        {
                            mapCell.ChipImage.sprite = FireStage3;
                        }
                    }
                }
            }
            
            if (nomber == 2)
            {
                foreach (MapCell mapCell in _secondBurningZone)
                {
                    mapCell.BurningDamage =+ 3;
                    if (PlayerSelector.Player.PlayerMapCell.id != mapCell.id && PlayerInferior.Player.PlayerMapCell.id != mapCell.id)
                    {
                        mapCell.SetAlphaChipSprite(1f);
                        if (mapCell.BurningDamage==3)
                        {
                            mapCell.ChipImage.sprite = FireStage1;
                        }
                        if (mapCell.BurningDamage==6)
                        {
                            mapCell.ChipImage.sprite = FireStage2;
                        }
                        if (mapCell.BurningDamage==9)
                        {
                            mapCell.ChipImage.sprite = FireStage3;
                        }
                    }
                }
            }
            
            if (nomber == 3)
            {
                foreach (MapCell mapCell in _thirdBurningZone)
                {
                    mapCell.BurningDamage =+ 3;
                    if (PlayerSelector.Player.PlayerMapCell.id != mapCell.id && PlayerInferior.Player.PlayerMapCell.id != mapCell.id)
                    {
                        mapCell.SetAlphaChipSprite(1f);
                        if (mapCell.BurningDamage==3)
                        {
                            mapCell.ChipImage.sprite = FireStage1;
                        }
                        if (mapCell.BurningDamage==6)
                        {
                            mapCell.ChipImage.sprite = FireStage2;
                        }
                        if (mapCell.BurningDamage==9)
                        {
                            mapCell.ChipImage.sprite = FireStage3;
                        }
                    }
                }
            }
        }
    }
}