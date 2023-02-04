using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardSpace
{
    [CreateAssetMenu(fileName = "NumberCard", menuName = "ScriptableObject/Cards/NumberCard", order = 2)]
    public class NumberCard : UnitCard
    {
        [BoxGroup("CardType")]
        [InfoBox("Increase the number of units.")]
        public CardTypeSmall cardTypeSmall = CardTypeSmall.Number;

        [BoxGroup("Unit Plus")]
        [VerticalGroup("Unit Plus/Stats")]
        [GUIColor(0.8f, 0.4f, 0.4f)]
        [LabelWidth(100)]
        [Min(0)]
        public int Warriors;
        [BoxGroup("Unit Plus")]
        [VerticalGroup("Unit Plus/Stats")]
        [GUIColor(0.5f, 1f, 0.5f)]
        [LabelWidth(100)]
        [Min(0)]
        public int Assasin;
        [BoxGroup("Unit Plus")]
        [VerticalGroup("Unit Plus/Stats")]
        [GUIColor(0.3f, 0.5f, 1f)]
        [LabelWidth(100)]
        [Min(0)]
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
                warrior = $"+{Warriors} WARRIOR\n";
            }
            
            string assasin = String.Empty;
            if (Assasin != 0)
            {
                assasin = $"+{Assasin} ASSASSIN\n";
            }
            
            string mage = String.Empty;
            if (Mage != 0)
            {
                mage = $"+{Mage} MAGE";
            }
            
            Description = $"{warrior}{assasin}{mage}";
        }
    }
}