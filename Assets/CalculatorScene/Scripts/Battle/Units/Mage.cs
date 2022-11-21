namespace TTBattle
{
    //Why parent IUnit?
    public class Mage : Unit, IUnit
    {
        public Mage()
        {
            Attack = 90;
            Health = 10;
            UnitType = UnitType.Mage;
        }

        //Zero usability
        public void MageSquadAttack(Squad squad, int Count)
        {
            Count = squad.Count;
        }
    }
}