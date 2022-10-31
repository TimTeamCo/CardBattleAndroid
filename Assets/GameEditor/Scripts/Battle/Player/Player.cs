using TTBattle.UI;
using UnityEngine;

namespace TTBattle
{
    public class Player : MonoBehaviour
    {
        public PlayerHand _playerHand;
        public bool _chooser;
        public float[] _unitsInfluence = new float [3];
        void Awake()
        {
            _playerHand = new PlayerHand();
                //_unitsInfluence = _playerMapCell.uintsInfluence;
        }
        
        
    }
}