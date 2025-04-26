using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class LeaderboardUploader : MonoBehaviour
{
    private string leaderboardUploadUrl = "http://localhost:3000/leaderboard-data";

    public IEnumerator UploadScore(int userId, int totalScore, int timeTaken)
    {
        string currentLevel = SceneManager.GetActiveScene().name;

        LeaderboardUploadData data = new LeaderboardUploadData
        {
            user_id = userId,
            total_score = totalScore,
            time_taken = timeTaken,
            level_name = currentLevel
        };

        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(leaderboardUploadUrl, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("? Score uploaded successfully");

            LeaderboardManager leaderboardManager = FindObjectOfType<LeaderboardManager>();
            if (leaderboardManager != null)
            {
                leaderboardManager.LoadLeaderboardForLevel(currentLevel);
            }
        }

    }

    [System.Serializable]
    public class LeaderboardUploadData
    {
        public int user_id;
        public int total_score;
        public int time_taken;
        public string level_name; 
    }
    }

