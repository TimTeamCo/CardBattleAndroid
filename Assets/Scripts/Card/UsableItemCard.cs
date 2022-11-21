using Sirenix.OdinInspector;
using UnityEngine;

namespace Card
{
    [CreateAssetMenu(fileName = "UsableItemCard", menuName = "ScriptableObject/Cards/UsableItem", order = 1)]
    public class UsableItemCard : ItemCard
    {
        [BoxGroup("CardType")]
        [InfoBox("Usable - Items that can be used in any turn.")]
        public CardTypeSmall cardTypeSmall = CardTypeSmall.Usable;
        
        [BoxGroup("Card Text")] [TextArea]
        public string Description;
    }
}