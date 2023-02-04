//using TTBattle.UI;
using UnityEngine;

namespace PlayerData
{
    [CreateAssetMenu(fileName = "PlayerDataCalculator", menuName = "ScriptableObject/Player/PlayerDataCalculator", order = 1)]
    public class PlayerDataCalculator : PlayerData
    {
        public Color PlayerColor;
        public Sprite PlayerChip;
        //public MapCell PlayerMapCell;
    }
}