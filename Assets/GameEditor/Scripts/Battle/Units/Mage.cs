namespace TTBattle
{
    public class Mage : Unit, IUnit
    {
        public Mage()
        {
            Attack = 90;
            Health = 10;
        }

        public void MageSquadAttack(Squad squad, int Count)
        {
            Count = squad.Count;
        }
    }
}