using Logic.Infrastructure;
using UnityEngine;

/// <summary>
/// Runs the countdown to the in-game state. While the start of the countdown is synced via Relay, the countdown itself is handled locally,
/// since precise timing isn't necessary.
/// </summary>
[RequireComponent(typeof(CountdownUI))]
public class Countdown : MonoBehaviour
{
    CallbackValue<float> TimeLeft = new CallbackValue<float>();

    private CountdownUI m_ui;
    private const int k_countdownTime = 10;

    public void OnEnable()
    {
        if (m_ui == null)
            m_ui = GetComponent<CountdownUI>();
        TimeLeft.onChanged += m_ui.OnTimeChanged;
        TimeLeft.Value = -1;
    }

    public void StartCountDown()
    {
        TimeLeft.Value = k_countdownTime;
    }

    public void CancelCountDown()
    {
        TimeLeft.Value = -1;
    }

    public void Update()
    {
        if (TimeLeft.Value < 0)
            return;
        TimeLeft.Value -= Time.deltaTime;
        if (TimeLeft.Value < 0)
            ApplicationController.Instance.GameManager.FinishedCountDown();
    }
}