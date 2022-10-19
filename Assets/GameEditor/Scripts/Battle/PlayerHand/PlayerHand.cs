using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle
{
    public class PlayerHand 
    {
        public Squad _warriorSquad = new Squad(new Warrior(), 10);
        public Squad _assasinSquad = new Squad(new Assasin(), 10);
        public Squad _mageSquad = new Squad(new Mage(), 10);
        private Squad[] playHand = new Squad[3];

        public Squad GetUnitChoice(int _dropdownValue)
        {
            playHand[0] = _warriorSquad;
            playHand[1] = _assasinSquad;
            playHand[2] = _mageSquad;
            return playHand[_dropdownValue];
        }
    }
}


