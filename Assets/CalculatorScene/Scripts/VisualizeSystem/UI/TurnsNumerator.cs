using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class TurnsNumerator : MonoBehaviour
    {
        [SerializeField] private Text _turnText;
        [SerializeField] private GameObject _defaultBattleButton;
        public int TurnsCount = 1;
        public int MoveCount;
        
        public void Numerate()
        {
            TurnsCount++;
            if (TurnsCount == 2)
            {
                _defaultBattleButton.SetActive(false);;
            }
            SetTurnText();
        }

        private void SetTurnText()
        {
            _turnText.text = $"{TurnsCount}";
        }
    }
}