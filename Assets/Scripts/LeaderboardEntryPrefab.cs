using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text usernameText;
    public TMP_Text scoreText;
    public TMP_Text timeText;

    public void Initialize(int rank, string username, int score, int time)
    {
        rankText.text = rank.ToString();
        usernameText.text = username;
        scoreText.text = score.ToString();
        timeText.text = time + "s";
    }
}
