namespace TTBattle
{
    public class Squad
    {
        private int _count;
        public Unit _unit;

        public Squad(Unit unit, int count)
        {
            Count = count;
            for (var i = 0; i < Count; i++) _unit = unit; //if _unit one why use for?
        }

        public int Count
        {
            get => _count;
            set => _count = value >= 0 ? value : 0;
        }
    }
}