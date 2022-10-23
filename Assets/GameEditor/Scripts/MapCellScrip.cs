using System;
using System.Collections;
using System.Collections.Generic;
using TTBattle.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(PolygonCollider2D))]
public class MapCellScrip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Image _image;
    [SerializeField] public int id;
    [SerializeField] public List<MapCellScrip> NextCell;
    public Color _selectColor;
    public Color _usualColor;
    public float _warriorInfluence;
    public float _assasinInfluence;
    public float _mageInfluence;
    public GameObject Map;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = _usualColor;
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        _warriorInfluence = (100 + _warriorInfluence)/100;
        _assasinInfluence = (100 + _assasinInfluence)/100;
        _mageInfluence = (100 + _mageInfluence)/100;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = _selectColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = _usualColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Map.GetComponent<TurnNumeratorButton>().MapCellPl1  = this;
    }
}

