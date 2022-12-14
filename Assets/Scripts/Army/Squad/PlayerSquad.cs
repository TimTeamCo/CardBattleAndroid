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
            set
            {
                if (value < 0)
                {
                    _count = 0;
                }
                else
                {
                    _count = value;
                }
            }
        }
    }
}