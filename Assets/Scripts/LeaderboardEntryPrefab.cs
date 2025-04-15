using UnityEngine;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text nameText;
    public TMP_Text scoreText;
    public TMP_Text timeText;

    public void SetRank(int rank)
    {
        rankText.text = rank.ToString();
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timeText.text = $"{minutes:00}:{seconds:00}";
    }
}
