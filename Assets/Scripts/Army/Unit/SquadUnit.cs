using Sirenix.OdinInspector;
using UnityEngine;

namespace Army
{
    [CreateAssetMenu(fileName = "SquadUnit", menuName = "ScriptableObject/SquadUnit/SquadUnit", order = 0)]
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
        [Range(0, 100)]
        [GUIColor(0.8f, 0.4f, 0.4f)]
        public int Attack;
        [VerticalGroup("Unit Data/Stats")]
        [LabelWidth(100)]
        [Range(0, 100)]
        [GUIColor(0.5f, 1f, 0.5f)]
        public int Health;
        
    }
}