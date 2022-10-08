using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTCalculator
{
    public class PlayerHand : MonoBehaviour
    {
        public Squad _warriorSquad = new Squad(new Warrior(), 10);
        public Squad _assasinSquad = new Squad(new Assasin(), 10);
        public Squad _mageSquad = new Squad(new Mage(), 10);
    }
}


