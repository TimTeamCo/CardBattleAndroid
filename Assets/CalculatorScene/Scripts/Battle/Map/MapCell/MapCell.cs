using System.Collections.Generic;
using Map;
using PlayerData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TTBattle.UI
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class MapCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] public List<MapCell> NextCell;
        [SerializeField] public Color ActiveChoiseColor;
        [SerializeField] public Color ChoisedCellColor;
        [SerializeField] public Color UsualColor;
        [SerializeField] public Image IndicateImage;
        [SerializeField] public MapZone MapZone;
        [SerializeField] public MapScript _map;
        [SerializeField] public bool IsAccasible;   
        [SerializeField] public bool IsTaken;
        [SerializeField] private Image _cellBG;
        
        private Color _lastColor;

        private void Awake()
        {
            _lastColor = UsualColor;
            _cellBG.color = _lastColor;
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
            SetAlphaChipSprite(0f);
            IndicateImage.transform.localScale = gameObject.transform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsAccasible && IsTaken && _map.MapCell != this)
            {
                SetImageColorToSelected();
                //_map.NewMapCell = _map.MapCell;
                _map.MapCell.IsAccasible = false;
                _map.NewMapCell = _map.MapCell;
                foreach (MapCell mapCell in _map.MapCell.NextCell)
                {
                    mapCell.IsAccasible = false;
                }

                _map.MakeTurn.ExecuteWithAttack();
                _map.NextCellInformer.gameObject.SetActive(false);
            }
            else if (IsAccasible && !IsTaken || _map.MapCell == this)
            {
                SetImageColorToSelected();
                _map.NewMapCell = this;
                _map.MakeTurn.MakeTurnButtonEnabled();
                _map.NextCellInformer.SetUnitsIfluenceText(this, true);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsAccasible == false) return;
            
            if (IsTaken)
            {
                _cellBG.color = ActiveChoiseColor;
                ActiveChoiseColor.a = 0.4f;
                _map.NextCellInformer.SetUnitsIfluenceText(this, false);
            }
            else
            {
                _cellBG.color = ActiveChoiseColor;
                _map.NextCellInformer.SetUnitsIfluenceText(this, false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsAccasible && !IsTaken)
            {
                _cellBG.color = _lastColor;
                _map.NextCellInformer.ExitCell();
            }

            if (IsAccasible && IsTaken)
            {
                ActiveChoiseColor.a = 1f;
                _cellBG.color = _lastColor;
                _map.NextCellInformer.ExitCell();
            }
        }

        public void CellIsLeaved()
        {
            _lastColor = UsualColor;
            _cellBG.color = _lastColor;
            IsTaken = false;
            IsAccasible = false;
            foreach (MapCell mapCell in NextCell)
            {
                mapCell.IsAccasible = false;
            }

            IndicateImage.sprite = null;
            IndicateImage.preserveAspect = false;
            SetFireSpriteToImage();
        }

        public void CellIsTaken()
        {
            IsTaken = true;
            IsAccasible = true;
            SetCellColorAsPlayers(_map.PlayerSelector.playerData);
            foreach (MapCell mapCell in NextCell)
            {
                mapCell.IsAccasible = true;
            }

            SetChipSpriteToImage(_map.PlayerSelector.playerData.PlayerChip);
        }

        public void SetCellColorAsPlayers(PlayerDataCalculator player)
        {
            _lastColor = player.PlayerColor;
            _lastColor.a = 0.8f;
            _cellBG.color = _lastColor;
        }

        public void SetImageColorToUsual()
        {
            _lastColor = UsualColor;
            _cellBG.color = _lastColor;
        }

        private void SetImageColorToSelected()
        {
            _lastColor = ChoisedCellColor;
            _cellBG.color = _lastColor;
        }

        public void SetChipSpriteToImage(Sprite chipSprite)
        {
            IndicateImage.sprite = chipSprite;
            IndicateImage.preserveAspect = true;
            IndicateImage.rectTransform.sizeDelta = new Vector2(145, 145);
            SetAlphaChipSprite(1f);
        }

        public void SetAlphaChipSprite(float f)
        {
            var tempColor = IndicateImage.color;
            tempColor.a = f;
            IndicateImage.color = tempColor;
        }

        public void SetFireSpriteToImage()
        {
            if (MapZone.burnFactor == 0)
            {
                SetAlphaChipSprite(0f);
            }
            else
            {
                IndicateImage.rectTransform.sizeDelta = new Vector2(130, 130);
                IndicateImage.sprite = MapZone.burnFactor switch
                {
                    3 => _map.FireStage1,
                    6 => _map.FireStage2,
                    9 => _map.FireStage3,
                    _ => IndicateImage.sprite
                };
            }
        }
    }
}