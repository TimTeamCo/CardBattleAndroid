using TTBattle.UI;
using UnityEngine;
using UnityEngine.UI;

public class BattleControllerView : MonoBehaviour
{
    [SerializeField] private Button _defaultBattleButton;
    [SerializeField] private ArmyPanel _army1;
    [SerializeField] private ArmyPanel _army2;

    private void Start()
    {
        _defaultBattleButton.onClick.AddListener(RefreshArmyStatsToDefault);
    }

    private void RefreshArmyStatsToDefault()
    {
        _army1.playerData.playerArmy.ResetToDefaultValue();
        _army2.playerData.playerArmy.ResetToDefaultValue();
    }
}
