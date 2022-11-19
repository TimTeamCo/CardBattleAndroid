using UnityEngine;

namespace Card
{
    [CreateAssetMenu(fileName = "Card", menuName = "ScriptableObject/Cards/Card", order = 0)]
    public class Card : ScriptableObject
    {
        public string name;
        public Sprite frame;
        public Sprite art;
    }
}