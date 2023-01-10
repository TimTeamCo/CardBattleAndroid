using CardSpace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
   [SerializeField] protected Image _frame;
   [SerializeField] protected Image _crystal;
   [SerializeField] protected Image _art;   
   [SerializeField] protected TextMeshProUGUI _name;
   [SerializeField] public Card _cardData;
   
   public virtual void OnValidate()
   {
       if (_cardData == null)
       {
           return;
       }
       
      _frame.sprite = _cardData.frame;
      _crystal.sprite = _cardData.crystal;
      _art.sprite = _cardData.art;
      _name.text = _cardData.cardName;
   }
   private void Start()
   {
        
   }
}
