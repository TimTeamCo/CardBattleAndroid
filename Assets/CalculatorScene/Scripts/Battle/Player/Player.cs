using UnityEngine;
using TTBattle.UI;

namespace TTBattle
{
    public class Player
    {
        public PlayerHand PlayerHand;
        public MapCell PlayerMapCell;
        public float[] UnitsInfluence = new float [3];
        public Color PlayerColor;
        public string PlayerName;
        public Sprite PlayerChip;

        public Player()
        {
            PlayerHand = new PlayerHand();
        }

        public void GetUnitsInfluence()
        {
            UnitsInfluence = PlayerMapCell.uintsInfluence;
        }

        public void BurningDamageToUnits()
        {
            if (PlayerMapCell.BurningDamage != 0)
            {
                PlayerHand._warriorSquad._unit.Health -= PlayerMapCell.BurningDamage;
                PlayerHand._assasinSquad._unit.Health -= PlayerMapCell.BurningDamage;
                PlayerHand._mageSquad._unit.Health -= PlayerMapCell.BurningDamage;
                Debug.Log(PlayerHand._warriorSquad._unit.Health);
            }
        }
     }
}