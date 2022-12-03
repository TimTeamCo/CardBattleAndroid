using System.Collections.Generic;
using UnityEngine;

namespace Army
{
    [CreateAssetMenu(fileName = "SquadSO", menuName = "ScriptableObject/Squad/SquadSO", order = 1)]
    public class PlayerSquads : ScriptableObject
    {
        public List<PlayerSquad> Squads;

        public void AddBurningDamageToUnits(int burningDamage)
        {
            foreach (var playerSquad in Squads)
            {
                playerSquad.SquadUnit.Health -= burningDamage;
            }
        }
    }
}