using Sirenix.OdinInspector;
using UnityEngine;

namespace Army
{
    [CreateAssetMenu(fileName = "SquadUnit", menuName = "ScriptableObject/Squad/SquadUnit", order = 0)]
    public class SquadUnit : ScriptableObject
    {
        [BoxGroup("Unit type")]
        [LabelWidth(100)]
        public UnitType UnitType;

        [PropertySpace(20)]
        [HorizontalGroup("Unit Data", 75)]
        [PreviewField(75, ObjectFieldAlignment.Left)]
        [HideLabel]
        public Sprite unitFace;
        
        [PropertySpace(20)]
        [VerticalGroup("Unit Data/Stats")]
        [LabelWidth(100)]
        [GUIColor(0.8f, 0.4f, 0.4f)]
        public int Attack;
        [VerticalGroup("Unit Data/Stats")]
        [LabelWidth(100)]
        [GUIColor(0.5f, 1f, 0.5f)]
        public int Health;

        public float WeakCoeficient = 0.5f;
        public float StrongCoefitient = 1.5f;

        [PropertySpace(50f)]
        [Button(Name = "RESET TO DEFAULT VALUE", Icon = SdfIconType.Backspace, Stretch = false, ButtonHeight = 50, IconAlignment = IconAlignment.LeftEdge)]
        public void ResetToDefaultValue()
        {
            switch (UnitType)
            {
                case UnitType.Warrior:
                    Attack = 10;
                    Health = 90;
                    break;
                case UnitType.Steamer:
                    Attack = 10;
                    Health = 30;
                    break;
                case UnitType.Mage:
                    Attack = 30;
                    Health = 30;
                    break;
            }

            WeakCoeficient = 0.5f;
            StrongCoefitient = 1.5f;
        }
    }
}