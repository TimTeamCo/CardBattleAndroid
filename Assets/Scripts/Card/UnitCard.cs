using Sirenix.OdinInspector;

namespace Card
{
    public class UnitCard : Card
    {
        [BoxGroup("CardType")]
        [InfoBox("Unit cards allow you to manipulate your army.")]
        public CardTypeBig cardTypeBig = CardTypeBig.Unit;
    }
}