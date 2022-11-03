namespace TTBattle
{
    public class Unit : IUnit
    {
        public UnitType UnitType;
        public const float strongCoefitient = 1.5f;
        public const float weakCoeficient = 0.5f;
        public int Health { get; set; } 
        public int Attack { get; set; }
        public float StrongCoefitient { get => strongCoefitient; }
        public float WeakCoeficient { get => weakCoeficient; }
    }

    public enum UnitType
    {
        Warrior,
        Assasin,
        Mage
    }
}