using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Army
{
    [CreateAssetMenu(fileName = "SquadSO", menuName = "ScriptableObject/Squad/SquadSO", order = 1)]
    public class PlayerArmy : ScriptableObject
    {
        public List<PlayerSquad> Squads;

        public void AddBurningDamageToUnits(int burningDamage)
        {
            foreach (var playerSquad in Squads)
            {
                playerSquad.SquadUnit.Health -= burningDamage;
            }
        }
        
        [PropertySpace(50f)]
        [Button(Name = "RESET TO DEFAULT VALUE", Icon = SdfIconType.Backspace, Stretch = false, ButtonHeight = 50, IconAlignment = IconAlignment.LeftEdge)]
        public void ResetToDefaultValue()
        {
            foreach (var playerSquad in Squads)
            {
                playerSquad.Count = 10;
                playerSquad.SquadUnit.ResetToDefaultValue();
            }
        }
    }
}