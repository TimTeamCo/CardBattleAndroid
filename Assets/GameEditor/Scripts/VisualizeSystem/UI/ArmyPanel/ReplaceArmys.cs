using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    //Army
    public class ReplaceArmys : MonoBehaviour
    {
        [SerializeField] private ArmyPanel _player1Army;
        [SerializeField] private ArmyPanel _player2Army;

        //Army
        public void DoReplaceArmys()
        {
            //Replace Replace Replace Replace Replace more Replays to Replays God
            ReplaceArmysValues();
            ReplaceDropdownValues();
            _player1Army.SetArmysValues();
            _player2Army.SetArmysValues();
        }

        /// <summary>
        /// Try use more simple in your logic. Optimize ur code and looked
        /// var _player1ArmyUnitDropdown = _player1Army._unitDropdown;
        /// var _player2ArmyUnitDropdown = _player2Army._unitDropdown;
        /// var _player1ArmyColor = _player1Army._unitDropdown.template.GetComponent<Image>().color;
        /// var _player2ArmyColor = _player2Army._unitDropdown.template.GetComponent<Image>().color;
        /// </summary>
        private void ReplaceDropdownValues()
        {
            (_player1Army._unitDropdown, _player2Army._unitDropdown) = (_player2Army._unitDropdown, _player1Army._unitDropdown);

            (_player1Army._unitDropdown.image.color, _player2Army._unitDropdown.image.color) = (_player2Army._unitDropdown.image.color, _player1Army._unitDropdown.image.color);

            (_player1Army._unitDropdown.template.GetComponent<Image>().color,
                _player2Army._unitDropdown.template.GetComponent<Image>().color) = (
                _player2Army._unitDropdown.template.GetComponent<Image>().color,
                _player1Army._unitDropdown.template.GetComponent<Image>().color);
        }

        /// <summary>
        /// name should be in _player1Army._player.name not in army also about playerColor
        /// Next why we just can't in params write (player1, player2)
        /// and us void Look like (54 line) uncommented
        /// </summary>
        private void ReplaceArmysValues()
        {
            var name = _player1Army.name;
            _player1Army.name = _player2Army.name;
            _player2Army.name = name;
            (_player1Army._player, _player2Army._player) = (_player2Army._player, _player1Army._player);
            (_player1Army._playerPanelColor, _player2Army._playerPanelColor) = (_player2Army._playerPanelColor, _player1Army._playerPanelColor);
        }

        // private void ReplaceArmy(Player player1, Player player2)
        // {
        //     //players change
        //     _player1Army._player = player1;
        //     _player2Army._player = player2;
        //     //name change no need and color change no need
        // }
    }
}