using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TTBattle.UI;

namespace TTBattle
    {
        public class Squad
        {
            public Unit _unit;
            private int _count;

            public int Count
            {
                get { return _count;}
                set
                {
                    _count = (value >= 0) ? value : 0;
                }
            }
            
            public Squad(Unit unit, int count)
            {
                Count = count;
                for (int i = 0; i < Count; i++)
                {
                    _unit = unit;
                }
            }

            
        }
    }
