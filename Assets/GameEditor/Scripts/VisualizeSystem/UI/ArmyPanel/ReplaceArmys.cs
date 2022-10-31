using UnityEngine;

namespace TTBattle.UI
{
    public class ReplaceArmys : MonoBehaviour
    {
        [SerializeField] private ArmyPanel _player1Army;
        [SerializeField] private ArmyPanel _player2Army;
        public void DoReplaceArmys()
        {
            ArmyPanel IntermediateArmy = _player1Army;
            _player1Army = _player2Army;
            _player2Army = IntermediateArmy;
            _player1Army.SetArmysData();
            _player2Army.SetArmysData();
        }
    }
}