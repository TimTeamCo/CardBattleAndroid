using TTGame;
using UnityEngine;

namespace TTUI
{
    [RequireComponent(typeof(LocalMenuStateObserver))]
    public class GameStateVisibilityUI  : ObserverPanel<LocalGameState>
    {
        [SerializeField] private GameState ShowThisWhen;
        
        public override void ObservedUpdated(LocalGameState observed)
        {
            if (ShowThisWhen.HasFlag(observed.State) == false)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
}