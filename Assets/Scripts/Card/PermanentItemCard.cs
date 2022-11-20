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

            char healthSign = ' ';
            if (Effect.HealthEffect > 0)
            {
                healthSign = '+';
            }else
            {
                healthSign = ' ';
            }

            string healthPart = String.Empty;
            if (healthSign != ' ')
            {
                healthPart = $"{healthSign} {Effect.HealthEffect} HP";
            }
            
            char attackSign = ' ';
            if (Effect.AttackEffect > 0)
            {
                attackSign = '+';
            }else
            {
                attackSign = ' ';
            }

            string attackPart = String.Empty;
            if (attackSign != ' ')
            {
                attackPart = $"{attackSign} {Effect.AttackEffect} ATK";
            }
            
            Description = $"{healthPart} {attackPart} FOR {Effect.UnitType.ToString().ToUpper()}S";
        }
    }
}