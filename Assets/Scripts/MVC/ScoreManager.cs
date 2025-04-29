using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TMP_Text scoreText;

    private float oxygenLevel = 100f;
    private int score = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool shouldReset = false;

    void Start()
    {
        if (shouldReset || PlayerPrefs.GetInt("restoreProgress", 0) == 0)
            ResetScore();
        else
        {
            score = PlayerPrefs.GetInt("totalScore", 0);
            oxygenLevel = PlayerPrefs.GetInt("oxygenLevel", 100);
            PlayerPrefs.SetInt("restoreProgress", 0);
            Debug.Log("Score and Oxygen restored.");
        }

        UpdateScoreDisplay();
    }


    void Update()
    {
        UpdateScoreDisplay(); 
    }

    public void SetOxygenLevel(float level)
    {
        oxygenLevel = level;
    }

    public int CalculateScore()
    {
        if (TimerManager.Instance == null)
            return score;

        float time = TimerManager.Instance.GetTime();
        score = Mathf.RoundToInt((oxygenLevel * 3f) + Mathf.Max(0, 300f - time));
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        oxygenLevel = 100f;
    }

    public void UpdateScoreDisplay()
    {
        score = CalculateScore();
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public int GetScore() => score;
}
