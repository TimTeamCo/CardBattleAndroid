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
        
        public Sprite FireStage1;
        public Sprite FireStage2;
        public Sprite FireStage3;
        public ArmyPanel PlayerSelector;
        public ArmyPanel PlayerInferior;
        public MapCell MapCell;
        public NextCellInformer NextCellInformer;

        public MapCell NewMapCell
        {
            get => _newMapCell;
            set
            { 
                if( _newMapCell == null)
                {
                    _newMapCell = value;
                    MakeTurn.MakeTurnButtonEnabled();
                }

                if (value.MapZone.zoneID == _newMapCell.MapZone.zoneID)
                {
                    return;
                }
                
                if (_newMapCell.IsTaken == false)
                { 
                    _newMapCell.SetImageColorToUsual();
                    _newMapCell = value;
                }
                else
                {
                    PlayerSelector.playerData.PlayerMapCell.SetCellColorAsPlayers(PlayerSelector.playerData);
                    _newMapCell = value;
                }
            }
        }
        
        private MapCell _secondRateMapCell;
        private MapCell _newMapCell;
        private MapCell _lastMapCell;
        
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
            InitializePLayersMapCells(PlayerSelector);
            InitializePLayersMapCells(PlayerInferior);
        }
        
        private void InitializePLayersMapCells(ArmyPanel player)
        {
            var playerCell = player.playerData.PlayerMapCell;
            playerCell = MapCell;
            PlayerSelector.playerData.MapZone = playerCell.MapZone;
        }
        
        private void SetPlayerInferiorMapCell()
        {
            {
                if (_newMapCell.MapZone.zoneID != MapCell.MapZone.zoneID)
                {
                    _lastMapCell = MapCell;
                    MapCell = NewMapCell;
                    MapCell.IsTaken = true;
                    MapCell.SetCellColorAsPlayers(PlayerInferior.playerData);
                    InitializePLayersMapCells(PlayerInferior);
                    MapCell.SetChipSpriteToImage(PlayerInferior.playerData.PlayerChip);
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
            MapCell = PlayerSelector.playerData.PlayerMapCell;
            MapCell.CellIsTaken();
            PlayerSelector.playerData.PlayerMapCell = MapCell;
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
                    mapCell.MapZone.burnFactor += 3;
                    if (PlayerSelector.playerData.PlayerMapCell.MapZone.zoneID != mapCell.MapZone.zoneID 
                        && PlayerInferior.playerData.PlayerMapCell.MapZone.zoneID != mapCell.MapZone.zoneID)
                    {
                        mapCell.SetAlphaChipSprite(1f);
                        mapCell.SetFireSpriteToImage();
                    }
                }
            }
            
            if (nomber == 2)
            {
                foreach (MapCell mapCell in _secondBurningZone)
                {
                    mapCell.MapZone.burnFactor =+ 3;
                    if (PlayerSelector.playerData.PlayerMapCell.MapZone.zoneID != mapCell.MapZone.zoneID 
                        && PlayerInferior.playerData.PlayerMapCell.MapZone.zoneID != mapCell.MapZone.zoneID)
                    {
                        mapCell.SetAlphaChipSprite(1f);
                        if (mapCell.MapZone.burnFactor==3)
                        {
                            mapCell.IndicateImage.sprite = FireStage1;
                        }
                        if (mapCell.MapZone.burnFactor==6)
                        {
                            mapCell.IndicateImage.sprite = FireStage2;
                        }
                        if (mapCell.MapZone.burnFactor==9)
                        {
                            mapCell.IndicateImage.sprite = FireStage3;
                        }
                    }
                }
            }
            
            if (nomber == 3)
            {
                foreach (MapCell mapCell in _thirdBurningZone)
                {
                    mapCell.MapZone.burnFactor =+ 3;
                    if (PlayerSelector.playerData.PlayerMapCell.MapZone.zoneID != mapCell.MapZone.zoneID 
                        && PlayerInferior.playerData.PlayerMapCell.MapZone.zoneID != mapCell.MapZone.zoneID)
                    {
                        mapCell.SetAlphaChipSprite(1f);
                        if (mapCell.MapZone.burnFactor==3)
                        {
                            mapCell.IndicateImage.sprite = FireStage1;
                        }
                        if (mapCell.MapZone.burnFactor==6)
                        {
                            mapCell.IndicateImage.sprite = FireStage2;
                        }
                        if (mapCell.MapZone.burnFactor==9)
                        {
                            mapCell.IndicateImage.sprite = FireStage3;
                        }
                    }
                }
            }
        }
    }
}