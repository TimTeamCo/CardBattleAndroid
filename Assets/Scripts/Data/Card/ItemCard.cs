﻿using Sirenix.OdinInspector;

namespace CardSpace
{
    public class ItemCard : Card
    {
        [BoxGroup("CardType")]
        [InfoBox("Item cards can strengthen an army or add an advantage in battle")]
        public CardTypeBig cardTypeBig = CardTypeBig.Item;
    }
}