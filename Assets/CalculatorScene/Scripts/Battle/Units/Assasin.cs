namespace TTBattle
{
    //The same IUnit
    public class Assasin: Unit, IUnit
    {
        public Assasin()
            {
                Attack = 30;
                Health = 30;
                UnitType = UnitType.Assasin;
            }
    }
}