using System;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class TurnsNumerator : MonoBehaviour
    {
        public int _numeratorValue = 1;

        public void Numerate()
        {
            _numeratorValue++;
            TurnNumeratorWiev();
        }

        public void TurnNumeratorWiev()
        {
            gameObject.GetComponent<Text>().text = Convert.ToString(_numeratorValue);
        }
    }
}