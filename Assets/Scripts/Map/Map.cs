using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "Map", menuName = "ScriptableObject/Map/Map", order = 0)]
    public class Map : ScriptableObject
    {
        public List<MapZone> _mapZones;
    }
}
