using System;
using UnityEngine;
using TTBattle.UI;
using UnityEngine.UI;

namespace TTBattle
{
    public class Player
    {
        public PlayerHand PlayerHand;
        public float[] UnitsInfluence = new float [3];
        public Color PlayerColor;
        public string PlayerName;
        public MapCell PlayerMapCell; 
        public Sprite PlayerChip;

        public Player()
        {
            PlayerHand = new PlayerHand();
        }

        public void GetUnitsInfluence()
        {
            UnitsInfluence = PlayerMapCell.uintsInfluence;
        }

        public void SetPlayerChipToCell()
        {
                PlayerMapCell.SetChipSprite(PlayerChip);
        }
    }
}