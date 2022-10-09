using System;

namespace TTGame
{
    [Flags]
    public enum GameState
    {
        Menu = 1,
        Searching = 2,
        Game = 3,
    }
    
    [Serializable]
    public class LocalGameState : Observed<LocalGameState>
    {
        GameState state = GameState.Menu;

        public GameState State
        {
            get => state;
            set
            {
                if (state != value)
                {
                    state = value;
                    OnChanged(this);
                }
            }
        }

        public override void CopyObserved(LocalGameState oldObserved)
        {
            if (state == oldObserved.State)
                return;
            state = oldObserved.State;
            OnChanged(this);
        }
    }
}