using System.Collections.Generic;
using UnityEngine;

namespace Army
{
    [CreateAssetMenu(fileName = "SquadSO", menuName = "ScriptableObject/Squad/SquadSO", order = 1)]
    public class PlayerSquads : ScriptableObject
    {
        public List<PlayerSquad> Squads;
    }
}