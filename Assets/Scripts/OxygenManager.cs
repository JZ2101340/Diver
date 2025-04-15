using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OxygenManager : MonoBehaviour
{
    public Slider oxygenSlider; // Reference to the oxygen slider UI
    public float oxygenDecreaseRate; // Rate at which oxygen decreases over time
    public float oxygenLostPerCreatureHit; // Amount of oxygen lost per creature hit
    private float maxOxygen = 100f; // Maximum oxygen level
    private float currentOxygen; // Current oxygen level

    public GameObject gameOverPanel; // Game over UI panel
    public Button retryButton; // Button for retrying the game
    public Button quitButton; // Button for quitting to the main menu
    public static OxygenManager Instance; // Singleton instance for easy access

    void Start()
    {
        currentOxygen = maxOxygen; // Set the initial oxygen level
        oxygenSlider.maxValue = maxOxygen; // Set the slider's max value
        oxygenSlider.value = currentOxygen; // Initialize the slider with the current oxygen level

        gameOverPanel.SetActive(false); // Hide the game over panel at the start

        // Assign button actions
        retryButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitToMainMenu);
    }

    void Update()
    {
        // Decrease oxygen over time
        currentOxygen -= oxygenDecreaseRate * Time.deltaTime;
        currentOxygen = Mathf.Max(0f, currentOxygen); // Prevent oxygen from going below 0
        oxygenSlider.value = currentOxygen;

        // Trigger Game Over if oxygen reaches 0
        if (currentOxygen <= 0)
        {
            GameOver();
        }
    }

    public void IncreaseOxygen(float amount)
    {
        // Increase oxygen and ensure it does not exceed the max value
        currentOxygen = Mathf.Min(currentOxygen + amount, maxOxygen);
        oxygenSlider.value = currentOxygen;
    }

    public void DecreaseOxygen(float amount)
    {
        // Decrease oxygen and ensure it does not go below 0
        currentOxygen -= amount;
        currentOxygen = Mathf.Max(0f, currentOxygen);
        oxygenSlider.value = currentOxygen;

        // Trigger Game Over if oxygen reaches 0
        if (currentOxygen <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // Handle game over logic
        Debug.Log("Player Ran Out of Oxygen! Game Over.");
        gameOverPanel.SetActive(true); // Show the game over UI panel
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        // Restart the current scene to retry the game
        Time.timeScale = 1f; // Resume the game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    void QuitToMainMenu()
    {
        // Quit to the main menu
        Time.timeScale = 1f; // Resume game time before quitting
        SceneManager.LoadScene("Main menu (login)"); // Make sure the scene name is correct
    }
}
