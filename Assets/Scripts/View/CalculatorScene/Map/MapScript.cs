using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class MapScript : MonoBehaviour
    {
        [SerializeField] private List<BurningZone> BurningZones = new List<BurningZone>();
        [SerializeField] public MakeTurn MakeTurn;
        [SerializeField] public ArmyPanelManager armyPanelManager;
        
        public Sprite FireStage1;
        public Sprite FireStage2;
        public Sprite FireStage3;
        [HideInInspector] public ArmyPanel PlayerSelector;
        [HideInInspector] public ArmyPanel PlayerInferior;
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
                    if (MakeTurn.IsAttack == false) return;
                    MakeTurn.UndoAttack();
                    //PlayerInferior.playerData.PlayerMapCell.SetBGImageToUsual();
                    return;
                }

                if (MakeTurn.IsAttack) MakeTurn.UndoAttack();
                
                _newMapCell.SetBGImageToUsual();
                _newMapCell = value;
            }
        }

        private MapCell _secondRateMapCell;
        private MapCell _newMapCell;
        private MapCell _lastMapCell;
        
        [Serializable]
        private struct BurningZone
        {
            public enum BurningIndicator
            {   
                FistBurningZone = 1,
                SecondBurningZone = 2,
                ThirdBurningZone = 3,
                ForthBurningZone = 4,
                FifthBurningZone = 5
            }

            public BurningIndicator OrderIndicator;
            public List<MapCell> BurningCells;
        }
        
        public void Awake()
        {
            SetPlayers();
            InitializePLayersMapCells();
        }

        private void Start()
        { 
            MapCell.CellIsTaken();
        }

        private void SetPlayers()
        {
            PlayerSelector = armyPanelManager.PlayerSelector;
            PlayerInferior = armyPanelManager.PlayerInferior;
        }
        
        private void InitializePLayersMapCells()
        {
            InitializePLayersMapCells(PlayerSelector);
            InitializePLayersMapCells(PlayerInferior);
        }
        
        private void InitializePLayersMapCells(ArmyPanel player)
        {
            //player.playerData.PlayerMapCell = MapCell;
            player.playerData.MapZone = MapCell.MapZone;
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
                    MapCell.SetChipSpriteToImage(PlayerInferior);
                    _lastMapCell.CellIsLeaved();
                    _newMapCell = null;
                }
                else
                {
                    //PlayerInferior.playerData.PlayerMapCell.IsAccasible = false;
                    foreach (MapCell mapCell in MapCell.NextCell)
                    {
                        mapCell.IsAccasible = false;
                    }
                    _newMapCell = null;
                    //PlayerInferior.playerData.PlayerMapCell.SetCellColorAsPlayers(PlayerInferior.playerData);
                }
            }
        }

        private void SetPlayerSelectorMapCell2()
        {
            //MapCell = PlayerSelector.playerData.PlayerMapCell;
            MapCell.CellIsTaken();
            //PlayerSelector.playerData.PlayerMapCell = MapCell;
        }
        
        public void SetPlayersMapCells()
        {
            SetPlayers();
            SetPlayerInferiorMapCell();
            SetPlayerSelectorMapCell2();
        }
        
        private void SetBurningCell(MapCell cell)
        {
            cell.MapZone.burnFactor += 3;
            /*if (PlayerSelector.playerData.PlayerMapCell.MapZone.zoneID != cell.MapZone.zoneID
                && PlayerInferior.playerData.PlayerMapCell.MapZone.zoneID != cell.MapZone.zoneID)
            {
                cell.SetAlphaChipSprite(1f);
                cell.SetFireSpriteToImage();
            }*/
        }

        public void SetBurningZones(int turnNumber)
        {
            foreach (BurningZone zone in BurningZones)
            {
                int indicator = (int) zone.OrderIndicator;
                switch (indicator)
                {
                    case 1:
                        if(turnNumber >= 5) 
                            foreach (MapCell cell in zone.BurningCells)
                            {
                                SetBurningCell(cell);
                            }
                        break;
                    case 2:
                        if(turnNumber >= 10) 
                            foreach (MapCell cell in zone.BurningCells)
                            {
                                SetBurningCell(cell);
                            }
                        break;
                    case 3:
                        if(turnNumber >= 15) 
                            foreach (MapCell cell in zone.BurningCells)
                            {
                                SetBurningCell(cell);
                            }
                        break;
                    case 4:
                        if(turnNumber >= 20) 
                            foreach (MapCell cell in zone.BurningCells)
                            {
                                SetBurningCell(cell);
                            }
                        break;
                    case 5:
                        if(turnNumber >= 25) 
                            foreach (MapCell cell in zone.BurningCells)
                            {
                                SetBurningCell(cell);
                            }
                        break;
                }
            }
        }
    }
}