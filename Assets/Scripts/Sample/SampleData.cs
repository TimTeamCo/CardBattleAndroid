using System.Collections.Generic;
using UnityEngine;

namespace Sample
{
    public class SampleData : ScriptableObject
    {
        public List<Sprite> icons;

        public Sprite GetIconForIndex(int index)
        {
            if (index < 0 || index >= icons.Count)
                index = 0;
            return icons[index];
        }
    }
}