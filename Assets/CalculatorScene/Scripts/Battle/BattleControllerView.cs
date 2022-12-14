using TTBattle.UI;
using UnityEngine;
using UnityEngine.UI;

public class BattleControllerView : MonoBehaviour
{
    [SerializeField] private MakeTurn _makeTurn;
    [SerializeField] private PlayerMenagerScript _playerMenagerScript;
    [SerializeField] private Button _defaultBattleButton;
    private ArmyPanel _armySelector;
    private ArmyPanel _armyInferrior;

    private void Start()
    {
        _defaultBattleButton.onClick.AddListener(RefreshArmyStatsToDefault);
    }

    private void RefreshArmyStatsToDefault()
    {
        SetPlayers();
        _armySelector.playerData.playerArmy.ResetToDefaultValue();
        _armyInferrior.playerData.playerArmy.ResetToDefaultValue();
        SetNewArmysValues();
        ReturnNewArmysValuesToTurnScript();
    }

    private void SetPlayers()
    {
        _armySelector = _playerMenagerScript.PlayerSelector;
        _armyInferrior = _playerMenagerScript.PlayerInferior;
    }

    private void SetNewArmysValues()
    {
        _armySelector.SetArmyValues();
        _armyInferrior.SetArmyValues();
        _armySelector.SetTextOfUnitsAmount();
        _armyInferrior.SetTextOfUnitsAmount();
    }

    private void ReturnNewArmysValuesToTurnScript()
    {
        _makeTurn.SetArmys();
    }
}
