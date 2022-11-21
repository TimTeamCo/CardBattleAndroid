using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card
{
    [CreateAssetMenu(fileName = "SacrificeCard", menuName = "ScriptableObject/Cards/SacrificeCard", order = 4)]
    public class SacrificeCard : UnitCard
    {
        [BoxGroup("CardType")]
        [InfoBox("Reducing the number of units to increase stats.")]
        public CardTypeSmall cardTypeSmall = CardTypeSmall.Sacrifice;
        
        [BoxGroup("Unit Crisis")]
        [VerticalGroup("Unit Crisis/Stats")]
        [GUIColor(0.8f, 0.4f, 0.4f)]
        [LabelWidth(50)]
        [HorizontalGroup("Unit Crisis/Stats/Warrior")]
        [MaxValue(0)]
        public int Warriors;
        
        [VerticalGroup("Unit Crisis/Stats/Warrior/New")]
        [LabelWidth(100)]
        [MinValue(0)]
        public int WarriorAttack;
        
        [VerticalGroup("Unit Crisis/Stats/Warrior/New")]
        [LabelWidth(100)]
        [MinValue(0)]
        public int WarriorHealth;
        
        [BoxGroup("Unit Crisis")]
        [VerticalGroup("Unit Crisis/Stats")]
        [GUIColor(0.5f, 1f, 0.5f)]
        [LabelWidth(50)]
        [HorizontalGroup("Unit Crisis/Stats/Assasin")]
        [MaxValue(0)]
        public int Assasin;
        
        [VerticalGroup("Unit Crisis/Stats/Assasin/New")]
        [LabelWidth(100)]
        [MinValue(0)]
        public int AssasinAttack;
        
        [VerticalGroup("Unit Crisis/Stats/Assasin/New")]
        [LabelWidth(100)]
        [MinValue(0)]
        public int AssasinHealth;
        
        [BoxGroup("Unit Crisis")]
        [VerticalGroup("Unit Crisis/Stats")]
        [GUIColor(0.3f, 0.5f, 1f)]
        [LabelWidth(50)]
        [HorizontalGroup("Unit Crisis/Stats/Mage")]
        [MaxValue(0)]
        public int Mage;
        
        [VerticalGroup("Unit Crisis/Stats/Mage/New")]
        [LabelWidth(100)]
        [MinValue(0)]
        public int MageAttack;
        
        [VerticalGroup("Unit Crisis/Stats/Mage/New")]
        [LabelWidth(100)]
        [MinValue(0)]
        public int MageHealth;
        
        [BoxGroup("Card Text")]
        [TextArea]
        [Multiline]
        public string Description;
        
        private void OnValidate()
        {
            string warrior = String.Empty;
            string assasin = String.Empty;
            string mage = String.Empty;

            if (Warriors != 0)
            {
                warrior = $"{Warriors} WARRIORS\n";
                if (WarriorAttack != 0)
                {
                    warrior += $"+{WarriorAttack} AT WAR\n";
                }
                
                if (WarriorHealth != 0)
                {
                    warrior += $"+{WarriorHealth} HP WAR\n";
                }
            }else if (Assasin != 0)
            {
                assasin = $"{Assasin} ASSASSINS\n";
                if (AssasinAttack != 0)
                {
                    assasin += $"+{AssasinAttack} AT ASN\n";
                }
                
                if (AssasinHealth != 0)
                {
                    assasin += $"+{AssasinHealth} HP ASN\n";
                }
            }else if (Mage != 0)
            {
                mage = $"{Mage} MAGES\n";
                if (MageAttack != 0)
                {
                    mage += $"+{MageAttack} AT MAG\n";
                }
                
                if (MageHealth != 0)
                {
                    mage += $"+{MageHealth} HP MAG\n";
                }
            }
            
            Description = $"{warrior}{assasin}{mage}";
        }
    }
}