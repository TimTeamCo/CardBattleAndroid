using System;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class PlayerTurnNumerator : MonoBehaviour
    {
        public  int _NumeratorValue = 1;
        [SerializeField] private Text _numeratorTextext;

        public void Numerate()
        {
            _NumeratorValue++;
            TurnNumeratorWiev();
        }

        public void TurnNumeratorWiev()
        {
            _numeratorTextext.text = Convert.ToString(_NumeratorValue);
        }

        public int GetNumeratorValue()
        {
            return _NumeratorValue;
        }
    }
}