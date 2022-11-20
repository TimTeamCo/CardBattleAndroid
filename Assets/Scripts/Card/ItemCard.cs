using Sirenix.OdinInspector;

namespace Card
{
    public class ItemCard : Card
    {
        [BoxGroup("CardType")] 
        public CardType CardType = CardType.Item;
    }
}