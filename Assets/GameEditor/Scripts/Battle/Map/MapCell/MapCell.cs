using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//using Image = UnityEngine.UI.Image;
namespace TTBattle.UI
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class MapCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] public List<MapCell> NextCell;
        [SerializeField] public int id;
        [SerializeField] private float _warriorInfluence;
        [SerializeField] private float _assasinInfluence;
        [SerializeField] private float _mageInfluence;
        [SerializeField] public Color _activeChoiseColor;
        [SerializeField] public Color _selectedCellColor;
        [SerializeField] public Color _usualColor;
        private Color _lastColor;
        private Image _image;
        private MapScript _map;
        [NonSerialized] public float[] uintsInfluence = new float [3];
        public bool _isAccasible = false;
        public bool _isSelected = false;
        private bool _occupaedAnoutherPlayer;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            _lastColor = _usualColor;
            _image.color = _lastColor;
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
            SetUnitsInfluence();
            _map = this.GetComponentInParent<MapScript>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(this._isSelected){}
            else if (this._isAccasible && this._occupaedAnoutherPlayer == true)
            {
                _lastColor = _selectedCellColor;
                _map._newMapCell = this;
            }
            else if (_isAccasible == true)
            {
                _lastColor = _selectedCellColor;
                _map.GetNewMapCell(this);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(this._isSelected){}
            else if (_isAccasible == true || (this._isAccasible && this._occupaedAnoutherPlayer == true))
            {
                _image.color = _activeChoiseColor;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isAccasible == true)
            {
                _image.color = _lastColor;
            }
        }

        private void SetUnitsInfluence()
        {
            uintsInfluence[0] = (100 + _warriorInfluence) / 100;
            uintsInfluence[1] = (100 + _assasinInfluence) / 100;
            uintsInfluence[2] = (100 + _mageInfluence) / 100;
        }

        public void LeaveCell()
        {
            _lastColor = _usualColor;
            _image.color = _lastColor;
            _isSelected = false;
            _isAccasible = false;
            foreach (MapCell mapCell in this.NextCell)
            {
                mapCell._isAccasible = false;
            }
        }
        
        public void CellIsSelected()
        {
            _isSelected = true;
            _isAccasible = true;
            _lastColor = _map._playerSelector._playerPanelColor;
            _lastColor.a = 0.8f;
            _image.color = _lastColor;
            foreach (MapCell mapCell in this.NextCell)
            {
                mapCell._isAccasible = true;
            }
        }
    }
}