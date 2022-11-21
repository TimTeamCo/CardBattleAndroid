using Sirenix.OdinInspector;

namespace Card
{
    public class ItemCard : Card
    {
        [BoxGroup("CardType")] 
        public CardTypeBig cardTypeBig = CardTypeBig.Item;
    }
}