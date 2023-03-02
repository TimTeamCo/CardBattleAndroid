//using TTBattle.UI;
using UnityEngine;

namespace PlayerDataSO
{
    [CreateAssetMenu(fileName = "PlayerDataCalculator", menuName = "ScriptableObject/Player/PlayerDataCalculator", order = 1)]
    public class PlayerDataSoCalculator : PlayerDataSO
    {
        public Color PlayerColor;
        public Sprite PlayerChip;
        //public MapCell PlayerMapCell;
    }
}