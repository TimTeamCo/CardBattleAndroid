using System;

namespace TTGame
{
    [Flags]
    public enum GameState
    {
        Menu = 1,
        Game = 2,
        Loading = 4,
    }
    
    [Serializable]
    public class LocalGameState : Observed<LocalGameState>
    {
        GameState state = GameState.Loading;

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