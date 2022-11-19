using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//using Image = UnityEngine.UI.Image; //?
namespace TTBattle.UI
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class MapCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {        
        [SerializeField] private float _warriorInfluence;
        [SerializeField] private float _assasinInfluence;
        [SerializeField] private float _mageInfluence;
        [SerializeField] public List<MapCell> NextCell;
        [SerializeField] public int id;
        [SerializeField] public Color ActiveChoiseColor;
        [SerializeField] public Color ChoisedCellColor;
        [SerializeField] public Color UsualColor;
        [SerializeField] public Image ChipImage;
        private Color _lastColor;
        private Image _image;
        public MapScript _map;
        public int BurningDamage;
        public float[] uintsInfluence = new float [3];
        public bool IsAccasible;
        public bool IsTaken;

        private void Awake()
        {
            _map = GetComponentInParent<MapScript>();
            _image = GetComponent<Image>();
            _lastColor = UsualColor;
            _image.color = _lastColor;
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
            SetUnitsInfluence();
            SetAlphaChipSprite(0f);
            ChipImage.transform.localScale = gameObject.transform.localScale;
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
            if (IsAccasible && !IsTaken)
            {
                _image.color = ActiveChoiseColor;
                _map.NextCellInformer.SetUnitsIfluenceText(this, false);
            }
            else if (IsAccasible && IsTaken)
                {
                    _image.color = ActiveChoiseColor;
                    ActiveChoiseColor.a = 0.4f;
                    _map.NextCellInformer.SetUnitsIfluenceText(this, false);
                }
            }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsAccasible && !IsTaken)
            {
                _image.color = _lastColor;
                _map.NextCellInformer.ExitCell();
            }

            if (IsAccasible && IsTaken)
            {
                ActiveChoiseColor.a = 1f;
                _image.color = _lastColor;
                _map.NextCellInformer.ExitCell();
            }
        }

        private void SetUnitsInfluence()
        {
            uintsInfluence[0] = (100 + _warriorInfluence) / 100;
            uintsInfluence[1] = (100 + _assasinInfluence) / 100;
            uintsInfluence[2] = (100 + _mageInfluence) / 100;
        }

        public void CellIsLeaved()
        {
            _lastColor = UsualColor;
            _image.color = _lastColor;
            IsTaken = false;
            IsAccasible = false;
            foreach (MapCell mapCell in NextCell)
            {
                mapCell.IsAccasible = false;
            }
            ChipImage.sprite = null;
            ChipImage.preserveAspect = false;
           SetFireSpriteToImage();
        }
        
        public void CellIsTaken()
        {
            IsTaken = true;
            IsAccasible = true;
            SetCellCollorAsPlayers(_map.PlayerSelector.Player);
            foreach (MapCell mapCell in NextCell)
            {
                mapCell.IsAccasible = true;
            }
            SetChipSprite(_map.PlayerSelector.Player.PlayerChip);
        }

        public void SetCellCollorAsPlayers(Player player)
        {
            _lastColor = player.PlayerColor;
            _lastColor.a = 0.8f;
            _image.color = _lastColor;
        }

        public void SetImageColorToUsual()
        {
            _lastColor = UsualColor;
            _image.color = _lastColor;
        }
        
        private void SetImageColorToSelected()
        {
            _lastColor = ChoisedCellColor;
            _image.color = _lastColor;
        }

        public void SetChipSprite(Sprite chipSprite)
        {
            ChipImage.sprite = chipSprite;
            ChipImage.preserveAspect = true;
            SetAlphaChipSprite(1f);
        }

        public void SetAlphaChipSprite(float f)
        {
            var tempColor = ChipImage.color;
            tempColor.a = f;
            ChipImage.color = tempColor;
        }

        public void SetFireSpriteToImage()
        {
            if (BurningDamage == 0)
            {
                SetAlphaChipSprite(0f);
            }
            else
            {
                if (BurningDamage == 3)
                {
                    ChipImage.sprite = _map.FireStage1;
                }
                if (BurningDamage == 6)
                {
                    ChipImage.sprite = _map.FireStage2;
                }
                if (BurningDamage == 9)
                {
                    ChipImage.sprite = _map.FireStage3;
                }
            }
        }
    }
}