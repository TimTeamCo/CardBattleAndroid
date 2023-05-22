using UnityEngine;

namespace View.Core
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private LobbyWindowView _lobbyWindowView;

        private void Start()
        {
            _lobbyWindowView.Init();
        }
    }
}