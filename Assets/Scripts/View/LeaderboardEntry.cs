using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text playerNameText;
    public TMP_Text scoreText;
    public TMP_Text timeText;

    public void Setup(int rank, string playerName, int score, string time)
    {
        rankText.text = rank.ToString();
        playerNameText.text = playerName;
        scoreText.text = score.ToString();
        timeText.text = time;
    }
}
