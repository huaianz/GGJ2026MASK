using UnityEngine;
using System;

public class Level2Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeLimitSeconds = 30f;

    private float timeLeft;
    private bool isRunning;

    public event Action OnTimeExpired;

    private void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            isRunning = false;
            timeLeft = 0f;
            OnTimeExpired?.Invoke();
        }
    }

    public void StartTimer()
    {
        timeLeft = timeLimitSeconds;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        StartTimer();
    }

    public float GetTimeLeft()
    {
        return timeLeft;
    }
}
