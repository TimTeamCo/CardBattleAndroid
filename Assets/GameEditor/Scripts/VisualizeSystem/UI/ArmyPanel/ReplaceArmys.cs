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
            _player1Army.SetArmyValues();
            _player2Army.SetArmyValues();
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
            (_player1Army.UnitDropdown, _player2Army.UnitDropdown) = (_player2Army.UnitDropdown, _player1Army.UnitDropdown);

            (_player1Army.UnitDropdown.image.color, _player2Army.UnitDropdown.image.color) = (_player2Army.UnitDropdown.image.color, _player1Army.UnitDropdown.image.color);

            (_player1Army.UnitDropdown.template.GetComponent<Image>().color,
                _player2Army.UnitDropdown.template.GetComponent<Image>().color) = (
                _player2Army.UnitDropdown.template.GetComponent<Image>().color,
                _player1Army.UnitDropdown.template.GetComponent<Image>().color);
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
            (_player1Army.Player, _player2Army.Player) = (_player2Army.Player, _player1Army.Player);
            (_player1Army.PlayerPanelColor, _player2Army.PlayerPanelColor) = (_player2Army.PlayerPanelColor, _player1Army.PlayerPanelColor);
        }

        private void ReplaceArmy(Player player1, Player player2)
        {
            _player1Army.Player = player1;
            _player2Army.Player = player2;
        }
    }
}