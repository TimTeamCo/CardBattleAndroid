using CardSpace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
   [SerializeField] private Image _frame;
   [SerializeField] private Image _crystal;
   [SerializeField] private Image _art;   
   [SerializeField] private TextMeshProUGUI _name;   
   [SerializeField] public Card _cardData;

   
   public void OnValidate()
   {
       if (_cardData == null)
       {
           return;
       }

        _cardData.ViewChangeAction += ChangeView;
   }

    private void ChangeView()
    {
        _frame.sprite = _cardData.frame;
        _crystal.sprite = _cardData.crystal;
        _art.sprite = _cardData.art;
        _name.text = _cardData.cardName;
        _cardData.ViewChangeAction -= ChangeView;
    }
}
