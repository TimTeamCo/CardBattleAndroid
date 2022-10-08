namespace TTCalculator
{
    public class Unit : IUnit
    {
        public const float strongCoefitient = 1.5f;
        public const float weakCoeficient = 0.5f;
        public int attack;
        public int health;

        public int Health
        {
            get => health;
            set
            {
                if (value != health)
                {
                    health = value;
                }
            }
        }

        public int Attack { get; set; }
        public float StrongCoefitient { get; }
        public float WeakCoeficient { get; }
        public virtual void AttackAction()
        {
                
        }
    }
}