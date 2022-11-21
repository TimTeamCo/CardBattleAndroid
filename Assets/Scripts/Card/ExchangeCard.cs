using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card
{
    [CreateAssetMenu(fileName = "ExchangeCard", menuName = "ScriptableObject/Cards/ExchangeCard", order = 3)]
    public class ExchangeCard : UnitCard
    {
        [BoxGroup("CardType")]
        [InfoBox("A significant increase in units of a certain type at the expense of units of another.")]
        public CardTypeSmall cardTypeSmall = CardTypeSmall.Exchange;

        [BoxGroup(" Unit Crisis")]
        [VerticalGroup(" Unit Crisis/Stats")]
        [GUIColor(0.8f, 0.4f, 0.4f)]
        [LabelWidth(100)]
        public int Warriors;
        [BoxGroup(" Unit Crisis")]
        [VerticalGroup(" Unit Crisis/Stats")]
        [GUIColor(0.5f, 1f, 0.5f)]
        [LabelWidth(100)]
        public int Assasin;
        [BoxGroup(" Unit Crisis")]
        [VerticalGroup(" Unit Crisis/Stats")]
        [GUIColor(0.3f, 0.5f, 1f)]
        [LabelWidth(100)]
        public int Mage;
        
        [BoxGroup("Card Text")]
        [TextArea]
        [Multiline]
        public string Description;
        
        private void OnValidate()
        {
            string warrior = String.Empty;
            if (Warriors != 0)
            {
                warrior = Warriors > 0 ? $"+{Warriors} WARRIOR\n" : $"{Warriors} WARRIOR\n";
            }
            
            string assasin = String.Empty;
            if (Assasin != 0)
            {
                assasin = Assasin > 0 ? $"+{Assasin} ASSASSIN\n" : $"{Assasin} ASSASSIN\n";
            }
            
            string mage = String.Empty;
            if (Mage != 0)
            {
                mage = Mage > 0 ? $"+{Mage} MAGE\n" : $"{Mage} MAGE\n";
            }
            
            Description = $"{warrior}{assasin}{mage}";
        }
    }
}