using Sirenix.OdinInspector;
using UnityEngine;

namespace Card
{
    public class Card : ScriptableObject
    {
        [BoxGroup("Basic Info")] 
        public int id;
        [BoxGroup("Basic Info")]
        public string name;
        
        [PreviewField(50, ObjectFieldAlignment.Left)]
        [BoxGroup("CardView")]
        [HorizontalGroup("CardView/Sprite", 150)]
        [LabelWidth(50)]
        public Sprite crystal;
        [PreviewField(100, ObjectFieldAlignment.Left)]
        [BoxGroup("CardView")]
        [HorizontalGroup("CardView/Sprite", 200)]
        [LabelWidth(50)]
        public Sprite frame;
        [PreviewField(100, ObjectFieldAlignment.Left)]
        [BoxGroup("CardView")]
        [HorizontalGroup("CardView/Sprite", 200)]
        [LabelWidth(50)]
        public Sprite art;
    }
}