using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score Display")]
    public TMP_Text scoreText;

    [Header("Score Weights")]
    public float oxygenWeight = 2f;
    public float timeWeight = 1f;

    private float oxygenLevel = 100f;
    private float timeTaken = 0f;
    private int score = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        timeTaken += Time.deltaTime;
        UpdateScoreDisplay();
    }

    public void SetOxygenLevel(float level)
    {
        oxygenLevel = level;
    }

    public void ResetTimer()
    {
        timeTaken = 0f;
    }

    public void StopTimer()
    {
        enabled = false;
    }

    public int CalculateScore()
    {
        score = Mathf.RoundToInt(oxygenLevel * oxygenWeight + timeTaken * timeWeight);
        return score;
    }

    public void UpdateScoreDisplay()
    {
        CalculateScore(); // Update score value
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public int GetScore() => score;
    public float GetTimeTaken() => timeTaken;
    public float GetOxygenLevel() => oxygenLevel;
}
