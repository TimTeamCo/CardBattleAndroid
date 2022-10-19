using UnityEngine;
using UnityEngine.UI;

namespace TTBattle.UI
{
    public class ArmyPanelCountUnitsWiew: MonoBehaviour

    {
    [SerializeField] public Text warrior;
    [SerializeField] public Text assasin;
    [SerializeField] public Text mage;
    public void SetCountUnitsPlayer(PlayerHand player)
    {
        warrior.text = player._warriorSquad.Count.ToString();
        assasin.text = player._assasinSquad.Count.ToString();
        mage.text = player._mageSquad.Count.ToString();
    }
    }
}