using System;
using System.Collections.Generic;
using Army;
using Map;
using TTBattle.UI;
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

        private void OnValidate()
        {
#if UNITY_EDITOR
            foreach (ArmyPanel armyPanel in FindObjectsOfType<ArmyPanel>())
            {
                if (armyPanel.playerData == this)
                {
                    armyPanel.ShowPlayerName();
                }
            }
#endif
        }
    }
}