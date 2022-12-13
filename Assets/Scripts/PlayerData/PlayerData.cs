using Army;
using Map;
using UnityEngine;

namespace PlayerData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/Player/Player", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public string PlayerName;
        public PlayerHand PlayerHand;
        public MapZone MapZone;
        public PlayerArmy playerArmy;
    }
}