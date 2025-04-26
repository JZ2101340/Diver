using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    public TMP_Text timerText;
    public float timeElapsed = 0f;
    private bool isRunning = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool shouldReset = false;

    void Start()
    {
        if (shouldReset || PlayerPrefs.GetInt("restoreProgress", 0) == 0)
            ResetTimer();
        else
        {
            timeElapsed = PlayerPrefs.GetInt("timeTaken", 0);
            Debug.Log("Timer Restored: " + timeElapsed);
        }
    }



    void Update()
    {
        if (!isRunning) return;

        timeElapsed += Time.deltaTime;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }
    public void ResetTimer()
    {
        timeElapsed = 0f;
        UpdateTimerUI();
    }

    public void SetTime(float time)
    {
        timeElapsed = time;
        UpdateTimerUI();
    }

    public void StopTimer() => isRunning = false;

    public float GetTime() => timeElapsed;
}
