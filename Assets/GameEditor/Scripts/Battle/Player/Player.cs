using UnityEngine;
using TTBattle.UI;

namespace TTBattle
{
    public class Player
    {
        public PlayerHand PlayerHand;
        public float[] UnitsInfluence = new float [3];
        public Color PlayerColor;
        public string PlayerName;
        public MapCell PlayerMapCell;


        public Player()
        {
            PlayerHand = new PlayerHand();
        }
    }
}