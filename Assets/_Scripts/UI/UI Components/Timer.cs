using System;
using TMPro;
using UnityEngine;

/// <summary>
///     Control class for an on-screen timer.
/// </summary>
[DisallowMultipleComponent]
public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText = null;
    [SerializeField] private Color timerActiveColor = Color.white;
    [SerializeField] private Color timerNotActiveColor = Color.white;

    [Tooltip("Time remaining in timer, represented in seconds")]
    [Range(0f, 300f)]
    [SerializeField] private float timeAmount = 150f;
    private float timeLeft = 0f;

    [Tooltip("Boolean value to control the format of the displayed time (MM:SS or S only)")]
    [SerializeField] private bool useMinutes = false;
    private bool isRunning = false;

    // Action to be invoked upon timer reaching 0
    private Action timerEndAction = null;



    public void Init(Action argOnComplete = null)
    {
        timeLeft = timeAmount;
        UpdateDisplayedTime();
        SetTimerActive(false);
        timerEndAction = argOnComplete;
    }

    public void Tick()
    {
        if (isRunning)
        {
            if (timeLeft > 0f)
            {
                timeLeft -= Time.deltaTime;
                UpdateDisplayedTime();
            }

            else
            {
                timeLeft = 0f;
                SetTimerActive(false);
                timerEndAction.InvokeNullCheck();
            }
        }
    }

    public void SetTimerActive(bool argValue)
    {
        timerText.color = argValue ? timerActiveColor : timerNotActiveColor;

        isRunning = argValue;
    }

    public void ResetTimer()
    {
        timeLeft = timeAmount;
        UpdateDisplayedTime();
        SetTimerActive(false);
    }

    private void UpdateDisplayedTime()
    {
        if (useMinutes)
        {
            float minutes = Mathf.FloorToInt((timeLeft + 1) / 60);
            float seconds = Mathf.FloorToInt((timeLeft + 1) % 60);

            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }

        else
        {
            timerText.text = $"{(int)(timeLeft + 1)}";
        }
    }
}