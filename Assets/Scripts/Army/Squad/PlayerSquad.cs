using System;

namespace Army
{
    [Serializable]
    public class PlayerSquad
    {
        private int _count;
        public SquadUnit SquadUnit;
        public int Count
        {
            get => _count;
            set => _count = (value >= 0)? value: 0;
        }
    }
}