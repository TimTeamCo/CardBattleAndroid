using TTBattle.UI;
using UnityEngine;

namespace TTBattle
{
    public class StartBattle : MonoBehaviour
    {
        [SerializeField] private ArmyPanel _player1Army;
        [SerializeField] private ArmyPanel _player2Army;
        [SerializeField] private MapScript _map;

        private void Awake()
        {
        }
    }
}