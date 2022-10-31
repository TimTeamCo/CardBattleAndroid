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
            ChangePlayersRoles();
        }

        private void ChangePlayersRoles()
        {
        }
    }
}