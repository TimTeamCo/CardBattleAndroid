using System;
using System.Collections.Generic;
using Army;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "MapZone0", menuName = "ScriptableObject/Map/MapZone", order = 0)]
    public class MapZone : ScriptableObject
    {
        public BiomType biom;
        public int zoneID ;
        public int burnFactor = 0;
        public List<BuffZone> buffsZone;

        [Serializable]
        public struct BuffZone
        {
            public UnitType unitType;
            public int buffValue;
        }
    }
}