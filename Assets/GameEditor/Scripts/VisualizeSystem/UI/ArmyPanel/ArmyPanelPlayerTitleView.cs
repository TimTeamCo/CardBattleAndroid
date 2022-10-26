using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class ArmyPanelPlayerTitleView : MonoBehaviour
    {
        [SerializeField] public GameObject Player;
        public Text NamePlayerText;
        void Start()
        {
            SetTitlePanel();
        }
        
        public void SetTitlePanel()
        {
            NamePlayerText.text = Player.name;
        }
    }
}