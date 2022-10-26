using TTBattle.UI;
using UnityEngine;

namespace TTBattle
{
    public class StartBattle : MonoBehaviour
    {
        private ArmyPanelCountUnitsWiew armyPanelCountUnitsWiewPlayer1;
        private ArmyPanelCountUnitsWiew armyPanelCountUnitsWiewPlayer2;
        //public PlayerHand _player1;
        //public PlayerHand _player2;
        [SerializeField] public GameObject Player1Panel;
        [SerializeField] public GameObject Player2Panel;
        [SerializeField] private GameObject Numerator;
        private PlayerTurnNumerator _playerTurnNumerator;
        public Player _player1;
        public Player _player2;

        private void Start()
        {
            _player1 = Player1Panel.GetComponent<Player>();
            _player2 = Player2Panel.GetComponent<Player>();
            SetArmysUnitsCount();
            _playerTurnNumerator = Numerator.GetComponent<PlayerTurnNumerator>();
            _playerTurnNumerator.TurnNumeratorWiev();
        }

        public void SetArmysUnitsCount()
        {
            armyPanelCountUnitsWiewPlayer1 = Player1Panel.GetComponent<ArmyPanelCountUnitsWiew>();
            armyPanelCountUnitsWiewPlayer1.SetCountUnitsPlayer(_player1._playerHand);
            armyPanelCountUnitsWiewPlayer2 = Player2Panel.GetComponent<ArmyPanelCountUnitsWiew>();
            armyPanelCountUnitsWiewPlayer2.SetCountUnitsPlayer(_player2._playerHand);
        }
    }
}