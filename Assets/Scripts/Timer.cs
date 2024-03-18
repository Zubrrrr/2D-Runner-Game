using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TMP_Text _timerText;
    private float startTime;
    private bool isTimerRunning = false;

    public void StartTimer()
    {
        startTime = Time.time;
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        StopTimer();
        startTime = Time.time; // Сброс начального времени
        UpdateTimerDisplay(); // Обновление отображения таймера
    }

    private void Start()
    {
        StartTimer();
        _timerText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        float timeElapsed = Time.time - startTime;
        _timerText.text = FormatTime(timeElapsed);
    }

    private string FormatTime(float timeInSeconds)
    {
        // Форматирование времени в формате ММ:СС
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
