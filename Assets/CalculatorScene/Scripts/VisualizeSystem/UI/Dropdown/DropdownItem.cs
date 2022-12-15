using System;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class DropdownItem : MonoBehaviour
    {
        [SerializeField] public int Value;
        [SerializeField] private DropdownCalculator _dropdownCalculator;
        [SerializeField] private Sprite _itemSprite;

        public void OnItemClick()
        {
            _dropdownCalculator.DropDownImage.sprite = _itemSprite;
            _dropdownCalculator.Value = Value;
            _dropdownCalculator.HideContent();
        }
    }
}