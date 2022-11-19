using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "MapZone", menuName = "ScriptableObject/Map/MapZone", order = 0)]
    public class MapZone : ScriptableObject
    {
        public BiomType biom;
    }
}