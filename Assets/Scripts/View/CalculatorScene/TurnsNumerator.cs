using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class TurnsNumerator : MonoBehaviour
    {
        [SerializeField] private Text _turnText;
        public int TurnsCount = 1;
        public int MoveCount;
        
        public void Numerate()
        {
            TurnsCount++;
            SetTurnText();
        }

        private void SetTurnText()
        {
            _turnText.text = $"{TurnsCount}";
        }
    }
}