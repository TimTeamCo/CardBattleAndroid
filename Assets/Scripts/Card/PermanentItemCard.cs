using System;
using Army;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card
{
    [CreateAssetMenu(fileName = "PermanentItemCard", menuName = "ScriptableObject/Cards/PermanentItem", order = 0)]
    public class PermanentItemCard : ItemCard
    {
        [BoxGroup("CardType")] public CardTypeSmall cardTypeSmall = CardTypeSmall.Permanent;

        [BoxGroup("Card Effect")] public int AttackEffect;
        [BoxGroup("Card Effect")] public int HealthEffect;
        [BoxGroup("Card Effect")] public UnitType UnitType;

        [BoxGroup("Card Text")] [TextArea]
        public string Description;

        private void OnValidate()
        {
            if (AttackEffect == 0 && HealthEffect == 0)
            {
                return;
            }

            string healthPart = String.Empty;
            if (HealthEffect != 0)
            {
                healthPart = HealthEffect > 0 ? $"+{HealthEffect} HP" : $"{HealthEffect} HP";
            }

            string attackPart = String.Empty;
            if (AttackEffect != 0)
            {
                attackPart = AttackEffect > 0 ? $"+{AttackEffect} ATK" : $"{AttackEffect} ATK";
            }

            Description = $"{healthPart} {attackPart} FOR {UnitType.ToString().ToUpper()}S";
        }
    }
}