using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//using Image = UnityEngine.UI.Image;
namespace TTBattle.UI
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class MapCellScrip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] public GameObject Map;
        [SerializeField] public int id;
        [SerializeField] public List<MapCellScrip> NextCell;
        [SerializeField] private float _warriorInfluence;
        [SerializeField] private float _assasinInfluence;
        [SerializeField] private float _mageInfluence;
        public Color _activeChoiseColor;
        public Color _selectedCellColor;
        public Color _usualColor;
        private Image _image;
        private Color _lastColor;
        [NonSerialized] public float[] uintsInfluence = new float [3];

        private void Awake()
        {
            _image = GetComponent<Image>();
            _lastColor = _usualColor;
            _image.color = _lastColor;
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
            SetUnitsInfluence();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Map.GetComponent<MapScript>().uintsInfluence = uintsInfluence;
            _lastColor = _selectedCellColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _image.color = _activeChoiseColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.color = _lastColor;
        }

        private void SetUnitsInfluence()
        {
            uintsInfluence[0] = (100 + _warriorInfluence) / 100;
            uintsInfluence[1] = (100 + _assasinInfluence) / 100;
            uintsInfluence[2] = (100 + _mageInfluence) / 100;
        }
    }
}