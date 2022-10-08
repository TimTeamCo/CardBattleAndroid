using System;
using System.Collections.Generic;
using UnityEngine;

namespace TTCalculator
{
    public class Calculate : MonoBehaviour
    {
        private PlayerHand _player1;
        private PlayerHand _player2;
        
        private void Start()
        {
            _player1 = new PlayerHand();
            _player2 = new PlayerHand();
        }
    }
}