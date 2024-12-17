using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text settingTimerText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject timerEnded;
    [SerializeField] private AudioSource timerEndedAudio;
    private float timerSeconds; // Total time in seconds
    private bool isRunning = false;
    
    private void Start()
    {
        timerSeconds = 0; // Initialize to zero
        UpdateTimerDisplay(timerSeconds); // Update display to "00:00"
    }

    public void StartOrResumeTimer()
    {
        if (timerSeconds > 0)
        {
            isRunning = true; // Start or resume the timer
        }
    }

    public void PauseTimer()
    {
        isRunning = false; // Pause the timer
    }

    public void DeleteTimer()
    {
        Destroy(transform.gameObject); // Destroy the GameObject
    }

    // Add 1 minute
    public void AddOneMinute()
    {
        timerSeconds += 60; // Add 1 minute (60 seconds)
        UpdateTimerDisplay(timerSeconds);
    }

    public void AddOneSecond()
    {
        timerSeconds += 1; // Add 1 second
        UpdateTimerDisplay(timerSeconds);
    }

    public void SubtractOneMinute()
    {
        timerSeconds -= 60; // Subtract 1 minute (60 seconds)
        timerSeconds = Mathf.Max(timerSeconds, 0); // Ensure timer does not go below zero
        UpdateTimerDisplay(timerSeconds);
    }

    public void SubtractOneSecond()
    {
        timerSeconds -= 1; // Subtract 1 second
        timerSeconds = Mathf.Max(timerSeconds, 0); // Ensure timer does not go below zero
        UpdateTimerDisplay(timerSeconds);
    }


    private void Update()
    {
        if (isRunning && timerSeconds > 0)
        {
            timerSeconds -= Time.deltaTime; // Decrease timer by the time elapsed
            timerSeconds = Mathf.Max(timerSeconds, 0); // Clamp to zero to avoid negative values

            UpdateTimerDisplay(timerSeconds); // Update the display

            if (timerSeconds <= 0)
            {
                isRunning = false; // Stop the timer
                OnTimerEnd(); // Handle timer end
            }
        }
    }

    private void UpdateTimerDisplay(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f); // Calculate minutes
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f); // Calculate seconds

        // Format and display the time as "MM:SS"
        timerText.text = $"{minutes:00}:{seconds:00}";
        settingTimerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnTimerEnd()
    {
        timerEnded.SetActive(true);
        timerEndedAudio.Play();
    }
}
