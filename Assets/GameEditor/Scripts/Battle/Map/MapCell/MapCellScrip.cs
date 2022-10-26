using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
//using Image = UnityEngine.UI.Image;
namespace TTBattle.UI
{
   [RequireComponent(typeof(PolygonCollider2D))]
   public class MapCellScrip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
   {
       [SerializeField] public GameObject Map;
       [SerializeField] public int id;
       [SerializeField] public List<MapCellScrip> NextCell;
       public Color _heilightColor;
       public Color _usualColor;
       public Color _activeCellColor;
       public float _warriorInfluence;
       public float _assasinInfluence;
       public float _mageInfluence;
       public float[] uintsInfluence = new float [3];
       private Image _image;
       private bool _playerAtCell;
       private Color _lastColor;
       
   
       
       private void Awake()
       {
           _image = GetComponent<Image>();
           _image.color = _usualColor;
           this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
           SetUnitsInfluence();
       }
   
       public void OnPointerEnter(PointerEventData eventData)
       {
           _image.color = _heilightColor;
       }
   
       public void OnPointerExit(PointerEventData eventData)
       {
           _image.color = _usualColor;
       }
   
       public void OnPointerDown(PointerEventData eventData)
       {
           Map.GetComponent<MapScript>().uintsInfluence = this.uintsInfluence;
           _playerAtCell = !_playerAtCell;
           _lastColor = _usualColor;
           _usualColor = _activeCellColor;
       }
   
       private void SetUnitsInfluence()
       {
           _warriorInfluence = (100 + _warriorInfluence)/100;
           _assasinInfluence = (100 + _assasinInfluence)/100;
           _mageInfluence = (100 + _mageInfluence)/100;
           uintsInfluence[0] = _warriorInfluence;
           uintsInfluence[1] = _assasinInfluence;
           uintsInfluence[2] = _mageInfluence;
       }
   } 
}


