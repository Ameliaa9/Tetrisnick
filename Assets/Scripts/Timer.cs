using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 80f; 
    private bool timerRunning = false;

    [SerializeField]
    TextMeshProUGUI timerText; 
    [SerializeField]
    Button restartButton; 

    private TetrisManager tetrisManager; 

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the TetrisManager script
        tetrisManager = FindObjectOfType<TetrisManager>();

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);  // Hide the restart button initially
            restartButton.onClick.AddListener(RestartGame);  // Add listener to restart button
        }

        StartTimer();  
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Decrease time
            UpdateTimerText(); // Update the UI text for timer

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                StopTimer(); // Stop the timer when it reaches 0
                GameOver(); // Handle game over when the timer runs out
            }
        }
    }

    // Start the timer
    public void StartTimer()
    {
        timerRunning = true;
    }

    // Stop the timer
    public void StopTimer()
    {
        timerRunning = false;
    }

    // Update the UI timer text
    private void UpdateTimerText()
    {
        timerText.text = "Time: " + Mathf.Max(0, Mathf.FloorToInt(timeRemaining)).ToString();
    }

    // This method is called when the timer runs out (game over condition)
    private void GameOver()
    {
        Debug.Log("Timer ran out. Game over!");

        // Show the restart button when game is over
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
            Debug.Log("Restart button is now active.");
        }

        // Inform the TetrisManager that the game is over and stop any game logic
        if (tetrisManager != null)
        {
            tetrisManager.GameOver();  // Call the GameOver method from TetrisManager
        }
    }

    // Restart the game when the button is clicked
    private void RestartGame()
    {
        Debug.Log("Restarting game...");

        // Reset timer and game state
        timeRemaining = 60f;
        StopTimer();
        StartTimer();

        // Reload the scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}