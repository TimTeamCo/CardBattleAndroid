using UnityEngine;

namespace TTBattle.UI
{
    public class DropdownItem : MonoBehaviour
    {
        [SerializeField] public int Value;
        [SerializeField] public Sprite _itemSprite;
        [SerializeField] private DropdownCalculator _dropdownCalculator;

        public void OnItemClick()
        {
            _dropdownCalculator.DropDownImage.sprite = _itemSprite;
            _dropdownCalculator.Value = Value;
            _dropdownCalculator.HideContent();
        }
    }
}