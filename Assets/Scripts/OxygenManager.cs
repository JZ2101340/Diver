using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OxygenManager : MonoBehaviour
{
    public static OxygenManager Instance;

    public Slider oxygenSlider;
    public float oxygenDecreaseRate;
    public GameObject gameOverPanel;
    public Button retryButton;
    public Button quitButton;

    private float maxOxygen = 100f;
    private float currentOxygen;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("restoreProgress", 0) == 1)
        {
            float restoredOxygen = PlayerPrefs.GetInt("oxygenLevel", 100);
            currentOxygen = restoredOxygen;
            Debug.Log("??? Restored oxygen: " + currentOxygen);
        }
        else
        {
            currentOxygen = maxOxygen;
            Debug.Log("??? New game, oxygen reset to: " + currentOxygen);
        }

        oxygenSlider.maxValue = maxOxygen;
        oxygenSlider.value = currentOxygen;

        // ? Reset timescale on level start
        Time.timeScale = 1f;

        // ? Hook up buttons
        retryButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitToMainMenu);

        gameOverPanel.SetActive(false);
    }



    void Update()
    {
        currentOxygen -= oxygenDecreaseRate * Time.deltaTime;
        currentOxygen = Mathf.Max(0f, currentOxygen);
        oxygenSlider.value = currentOxygen;

        ScoreManager.Instance.SetOxygenLevel(currentOxygen); // ? Ensure real-time score updates

        if (currentOxygen <= 0)
        {
            GameOver();
        }
    }


    public void IncreaseOxygen(float amount)
    {
        currentOxygen = Mathf.Min(currentOxygen + amount, maxOxygen);
        oxygenSlider.value = currentOxygen;
        ScoreManager.Instance.SetOxygenLevel(currentOxygen);
    }

    public void DecreaseOxygen(float amount)
    {
        currentOxygen = Mathf.Max(0f, currentOxygen - amount);
        oxygenSlider.value = currentOxygen;
        ScoreManager.Instance.SetOxygenLevel(currentOxygen);

        if (currentOxygen <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("?? Game Over");
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // ? Freeze the game
        SaveFinalProgress();

    }



    void SaveFinalProgress()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (NetworkManager.Instance != null &&
            System.Array.Exists(NetworkManager.levelScenes, scene => scene == currentScene))
        {
            NetworkManager.Instance.StartCoroutine(
                NetworkManager.Instance.UpdateProgress(
                    currentScene,
                    Mathf.FloorToInt(currentOxygen),
                    Mathf.FloorToInt(TimerManager.Instance.GetTime()),
                    CheckpointManager.Instance.GetReachedCheckpointCount(),
                    ScoreManager.Instance.GetScore()
                )
            );
        }
    }


    public void RestartGame()
    {
        Debug.Log("?? Restarting level");
        Time.timeScale = 1f; // ? Unpause time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
                                             
    }

    void QuitToMainMenu()
    {
        Debug.Log("?? Going to main menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main menu (login)"); // ? Replace with actual main menu name
    }


    public float GetOxygenLevel() => currentOxygen;
}
