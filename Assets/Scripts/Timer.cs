using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    public TMP_Text timerText;
    private float timer = 0f;
    private bool isRunning = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }

    public void StopTimer() => isRunning = false;
    public float GetTime() => timer;
}
