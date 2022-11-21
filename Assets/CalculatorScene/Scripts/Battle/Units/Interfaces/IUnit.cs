namespace TTBattle
{
    public interface IUnit
    {
        public int Health { get; set; }
        public int Attack { get; set; }
        public float StrongCoefitient { get; }
        public float WeakCoeficient { get; }
    }
}