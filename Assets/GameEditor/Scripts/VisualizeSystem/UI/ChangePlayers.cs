using UnityEngine;

namespace TTBattle.UI
{
    public class ChangePlayers : MonoBehaviour
    {
        [SerializeField] public GameObject Army1;
        [SerializeField] public GameObject Army2; 
        private ArmyPanelPlayerTitleView _armyPanelPlayerTitleViewArmy1;
        private ArmyPanelPlayerTitleView _armyPanelPlayerTitleViewArmy2;
        [SerializeField] public GameObject Battle;
        private string _titleArmy1;
        private string _titleArmy2;
        private PlayerHand _player1;
        private PlayerHand _player2;

        public void DoChangeArmys()
        {
            ChangeArmysCount();
            ChangePlayersTitle();
        }

        private void ChangeArmysCount()
        {
            Army1.GetComponent<ArmyPanelCountUnitsWiew>();
            _player1 = Battle.GetComponent<StartBattle>()._player1._playerHand;
            _player2 = Battle.GetComponent<StartBattle>()._player2._playerHand;
            Battle.GetComponent<StartBattle>()._player1._playerHand = _player2;
            Battle.GetComponent<StartBattle>()._player2._playerHand = _player1;
            Battle.GetComponent<StartBattle>().SetArmysUnitsCount();
        }

        private void ChangePlayersTitle()
        {
            _titleArmy1 = Army1.GetComponent<ArmyPanelPlayerTitleView>().Player.name;
            _titleArmy2 = Army2.GetComponent<ArmyPanelPlayerTitleView>().Player.name;
            Army1.GetComponent<ArmyPanelPlayerTitleView>().Player.name = _titleArmy2;
            Army2.GetComponent<ArmyPanelPlayerTitleView>().Player.name = _titleArmy1;
            Army1.GetComponent<ArmyPanelPlayerTitleView>().SetTitlePanel();
            Army2.GetComponent<ArmyPanelPlayerTitleView>().SetTitlePanel();
        }
    }
}