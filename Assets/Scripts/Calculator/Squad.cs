using System.Collections.Generic;

namespace TTCalculator
    {
        public class Squad
        {
            private List<Unit> _units = new List<Unit>();
            public int _count;
            
            public Squad(Unit unit, int count)
            {
                _count = count;
                for (int i = 0; i < _count; i++)
                {
                    _units.Add(unit);
                }
            }
        }
    }
