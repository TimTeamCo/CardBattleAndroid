using System.Collections.Generic;
using CardSpace;
using UnityEngine;

namespace PlayerDataSO
{
    [CreateAssetMenu(fileName = "PlayerHand", menuName = "ScriptableObject/Player/PlayerHand", order = 1)]
    public class PlayerHand : ScriptableObject
    {
        public List<Card> playerCards;
    }
}