using System;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class TurnsNumerator : MonoBehaviour
    {
        // really need public?
        public int _numeratorValue = 1;

        public void Numerate()
        {
            _numeratorValue++;
            TurnNumeratorWiev();
        }

        //View
        public void TurnNumeratorWiev()
        {
            // bad need cache all GetComponent in Start or Serialize
            gameObject.GetComponent<Text>().text = Convert.ToString(_numeratorValue);
        }
    }
}