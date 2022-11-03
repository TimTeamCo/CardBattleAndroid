using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class TurnsNumerator : MonoBehaviour
    {
        [SerializeField] private Text _turnText;
        public int NumeratorValue = 1;
        
        public void Numerate()
        {
            NumeratorValue++;
            SetTurnText();
        }

        private void SetTurnText()
        {
            _turnText.text = $"{NumeratorValue}";
        }
    }
}