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
        [PreviewField(100)]
        [BoxGroup("Basic Info")]
        public Sprite frame;
        [PreviewField(100)]
        [BoxGroup("Basic Info")]
        public Sprite art;
    }
}