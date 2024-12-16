using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private float elapsedTime;      // Time in seconds
    private bool isRunning = false; // To track if the timer is active

    // Start or resume the timer
    public void StartOrResumeTimer()
    {
        isRunning = true; // Activate the timer
    }

    // Pause the timer
    public void PauseTimer()
    {
        isRunning = false; // Deactivate the timer
    }

    // Restart the timer
    public void RestartTimer()
    {
        isRunning = true;  // Activate the timer
        elapsedTime = 0f;  // Reset the elapsed time
        UpdateTimerDisplay(0); // Update the display to "00:00"
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime; // Add the time since last frame
            UpdateTimerDisplay(elapsedTime); // Update the timer display
        }
    }

    // Update the timer display
    private void UpdateTimerDisplay(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f); // Calculate minutes
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f); // Calculate seconds

        // Format and display the time as "MM:SS"
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}