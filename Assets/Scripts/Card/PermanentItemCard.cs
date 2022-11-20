using System;
using Army;
using UnityEngine;

namespace Card
{
    [CreateAssetMenu(fileName = "PermanentItemCard", menuName = "ScriptableObject/Cards/PermanentItemCard", order = 0)]
    public class PermanentItemCard : ItemCard
    {
        public Effects Effect;
        public string Description;
        
        [Serializable]
        public struct Effects
        {
            public int AttackEffect;
            public int HealthEffect;
            public UnitType UnitType;
        }

        private void OnValidate()
        {
            if (Effect.AttackEffect == 0 && Effect.HealthEffect == 0)
            {
                return;
            }
            
            string healthPart = String.Empty;
            if (Effect.HealthEffect != 0)
            {
                healthPart = Effect.HealthEffect > 0 ? $"+{Effect.HealthEffect} HP" : $"{Effect.HealthEffect} HP";
            }
            
            string attackPart = String.Empty;
            if (Effect.AttackEffect != 0)
            {
                attackPart = Effect.AttackEffect > 0 ? $"+{Effect.AttackEffect} ATK" : $"{Effect.AttackEffect} ATK";
            }

            Description = $"{healthPart} {attackPart} FOR {Effect.UnitType.ToString().ToUpper()}S";
        }
    }
}