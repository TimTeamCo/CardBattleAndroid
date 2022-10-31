using System;
using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    [RequireComponent(typeof(Button))]
    public class ChooseMapCellButton : MonoBehaviour
    {
        public Button a;
        [SerializeField] private Player _player;

        public void PressedButton()
        {
            ChangeChoosers();
        }

        private void ChangeChoosers()
        {
            _player._chooser = !_player._chooser;
        }
    }
}