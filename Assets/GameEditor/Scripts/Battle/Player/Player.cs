namespace TTBattle
{
    public class Player
    {
        public PlayerHand _playerHand;
        public float[] _unitsInfluence = new float [3];

        public Player()
        {
            _playerHand = new PlayerHand();
        }
    }
}