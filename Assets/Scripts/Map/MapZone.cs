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
        public List<MapZone> neighboringAreas;
        private Dictionary<UnitType, int> _unitBuffValue = new Dictionary<UnitType, int>();

        [Serializable]
        public struct BuffZone
        {
            public UnitType unitType;
            public int buffValue;
        }

        private void OnValidate()
        {
            _unitBuffValue.Clear();
            foreach (var buffZone in buffsZone)
            {
                _unitBuffValue.Add(buffZone.unitType, buffZone.buffValue);
            }
            Debug.Log($"Count in dictionary buffs = {_unitBuffValue.Count}");
        }

        public float GetUnitInfluence(UnitType unitType)
        {
            float influence = 0; 
            switch (unitType)
            {
                case UnitType.Warrior:
                    influence = (100 + _unitBuffValue[UnitType.Warrior]) / 100;
                    break;
                case UnitType.Steamer:
                    influence = (100 + _unitBuffValue[UnitType.Steamer]) / 100;
                    break;
                case UnitType.Mage:
                    influence = (100 + _unitBuffValue[UnitType.Mage]) / 100;
                    break;
            }

            return influence;
        }
    }
}