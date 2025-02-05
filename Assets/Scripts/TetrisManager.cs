using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class TetrisManager : MonoBehaviour
{
    private tetrisGrid grid;
    public int score;

    [SerializeField]
    GameObject gameOverText;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI highScoreText;
    [SerializeField]
    TetrisSpawner tetrisSpawner;

    [SerializeField]
    Button restartButton;
    [SerializeField]
    Timer timerScript;

    [SerializeField]
    GameObject gameOverPic;

    private int highScore;
    private bool gameOver = false; // To track if the game is over

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<tetrisGrid>();

        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Set initial high score text
        UpdateHighScoreText();

        // Ensure the restart button is hidden at the start
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
            restartButton.onClick.AddListener(RestartGame);  // Add listener to restart button
        }

        // Ensure the GameOverPic is hidden at the start
        if (gameOverPic != null)
        {
            gameOverPic.SetActive(false);
        }

        // Ensure the high score text is hidden at the start
        if (highScoreText != null)
        {
            highScoreText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)  // Only check game over if game is not already over
        {
            CheckGameOver();
        }

        scoreText.text = "Score: " + score;
    }

    // Function to calculate score based on cleared lines
    public void CalculateScore(int linesCleared)
    {
        switch (linesCleared)
        {
            case 1:
                score += 100;
                break;
            case 2:
                score += 300;
                break;
            case 3:
                score += 500;
                break;
            case 4:
                score += 800;
                break;
        }

        // Update high score if the current score is greater
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);  // Save to PlayerPrefs
            PlayerPrefs.Save();
            UpdateHighScoreText();
        }
    }

    // Update the high score text on screen
    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
    }

    public void CheckGameOver()
    {
        for (int i = 0; i < grid.width; i++)
        {
            if (grid.IsCellOccupied(new Vector2Int(i, grid.height - 1)))
            {
                Debug.Log("Game over triggered!");

                gameOverText.SetActive(true);  // Show the game over text
                highScoreText.gameObject.SetActive(true); // Show high score on game over screen
                tetrisSpawner.gameObject.SetActive(false); // Disable TetrisSpawner

                if (gameOverPic != null)
                {
                    gameOverPic.SetActive(true);
                }

                if (restartButton != null)
                {
                    restartButton.gameObject.SetActive(true);
                }

                gameOver = true;

                if (timerScript != null)
                {
                    timerScript.StopTimer();
                }

                break;
            }
        }
    }

    // This method is called when the game is over
    public void GameOver()
    {
        Debug.Log("Game over triggered by timer!");

        gameOverText.SetActive(true);
        highScoreText.gameObject.SetActive(true); // Show high score when game is over
        tetrisSpawner.gameObject.SetActive(false);

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
        }

        if (gameOverPic != null)
        {
            gameOverPic.SetActive(true);
        }

        if (timerScript != null)
        {
            timerScript.StopTimer();
        }
    }


    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        score = 0;  // Reset score
        gameOverText.SetActive(false);
        highScoreText.gameObject.SetActive(false);  // Hide high score when restarting
        tetrisSpawner.gameObject.SetActive(true);

        if (gameOverPic != null)
        {
            gameOverPic.SetActive(false);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }
}