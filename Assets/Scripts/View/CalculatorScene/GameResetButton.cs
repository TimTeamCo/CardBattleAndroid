using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResetButton : MonoBehaviour
{
    [SerializeField] private ArmyPanelManager _armyPanelManager;
    [SerializeField] private Button _defaultBattleButton;

    private void Start()
    {
        _defaultBattleButton.onClick.AddListener(GameReset);
    }

    private void GameReset()
    {
        _armyPanelManager.PlayerInferior.playerDataSo.playerArmy.ResetToDefaultValue();
        _armyPanelManager.PlayerSelector.playerDataSo.playerArmy.ResetToDefaultValue();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
