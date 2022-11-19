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
        public List<UnitType> UnitTypes;
        public List<int> buffValue;
        public int burnFactor = 0;
        public Dictionary<UnitType, int> BuffZone;

        private void OnValidate()
        {
            if (UnitTypes.Count != buffValue.Count)
            {
                return;
            }
            
            BuffZone = new Dictionary<UnitType, int>();
            for (int i = 0; i < UnitTypes.Count; i++)
            {
                BuffZone.Add(UnitTypes[i], buffValue[i]);
            }
            Debug.Log($"New Dic {BuffZone.Keys.Count} {BuffZone.Values.Count}");
        }
    }
}