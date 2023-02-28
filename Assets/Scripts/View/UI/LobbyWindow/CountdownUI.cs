using TMPro;
using UnityEngine;

/// <summary>
/// After all players ready up for the game, this will show the countdown that occurs.
/// This countdown is purely visual, to give clients a moment if they need to un-ready before entering the game;
/// clients will actually wait for a message from the host confirming that they are in the game, instead of assuming the game is ready to go when the countdown ends.
/// </summary>
public class CountdownUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _countDownText;

    public void OnTimeChanged(float time)
    {
        if (time < 0)
        {
            _countDownText.text = "";
        }
        else
        {
            _countDownText.text = $"{time:0}";
        }
    }
}