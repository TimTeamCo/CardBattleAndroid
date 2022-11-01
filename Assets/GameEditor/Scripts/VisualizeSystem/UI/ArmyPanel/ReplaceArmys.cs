using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class ReplaceArmys : MonoBehaviour
    {
        [SerializeField] private ArmyPanel _player1Army;
        [SerializeField] private ArmyPanel _player2Army;

        public void DoReplaceArmys()
        {
            ReplaceArmysValues();
            ReplaceDropdownValues();
            _player1Army.SetArmysValues();
            _player2Army.SetArmysValues();
        }

        private void ReplaceDropdownValues()
        {
            (_player1Army._unitDropdown, _player2Army._unitDropdown) =
                (_player2Army._unitDropdown, _player1Army._unitDropdown);

            (_player1Army._unitDropdown.image.color, _player2Army._unitDropdown.image.color) = (
                _player2Army._unitDropdown.image.color, _player1Army._unitDropdown.image.color);

            (_player1Army._unitDropdown.template.GetComponent<Image>().color,
                _player2Army._unitDropdown.template.GetComponent<Image>().color) = (
                _player2Army._unitDropdown.template.GetComponent<Image>().color,
                _player1Army._unitDropdown.template.GetComponent<Image>().color);
        }

        private void ReplaceArmysValues()
        {
            var name = _player1Army.name;
            _player1Army.name = _player2Army.name;
            _player2Army.name = name;
            (_player1Army._player, _player2Army._player) = (_player2Army._player, _player1Army._player);
            (_player1Army._playerPanelColor, _player2Army._playerPanelColor) = (_player2Army._playerPanelColor, _player1Army._playerPanelColor);
        }
    }
}