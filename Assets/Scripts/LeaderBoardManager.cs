using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public Transform content;
    public GameObject leaderboardEntryPrefab;
    private string serverBaseUrl = "http://localhost:3000/leaderboard"; // Base URL

    [System.Serializable]
    public class LeaderboardEntryData
    {
        public int rank;
        public string username;
        public int total_score;
        public int time_taken;
    }

    [System.Serializable]
    public class LeaderboardEntriesList
    {
        public List<LeaderboardEntryData> entries;
    }

    private void Start()
    {
        // Load default leaderboard at start, for example Easy
        LoadLeaderboardForLevel("Easy");
    }

    public void LoadLeaderboardForLevel(string levelName)
    {
        StartCoroutine(LoadLeaderboard(levelName));
    }

    public IEnumerator LoadLeaderboard(string levelName)
    {
        string url = serverBaseUrl + "/" + levelName;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = "{\"entries\":" + request.downloadHandler.text + "}";
            LeaderboardEntriesList entriesList = JsonUtility.FromJson<LeaderboardEntriesList>(json);

            PopulateLeaderboard(entriesList.entries);
        }
        else
        {
            Debug.LogError("Error fetching leaderboard: " + request.error);
        }
    }

    private void PopulateLeaderboard(List<LeaderboardEntryData> leaderboardEntries)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in leaderboardEntries)
        {
            GameObject newEntry = Instantiate(leaderboardEntryPrefab, content);
            LeaderboardEntry entryScript = newEntry.GetComponent<LeaderboardEntry>();
            if (entryScript != null)
            {
                string formattedTime = FormatTime(entry.time_taken);
                entryScript.Setup(entry.rank, entry.username, entry.total_score, formattedTime);
            }
        }
    }

    private string FormatTime(int seconds)
    {
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;
        return minutes.ToString("00") + ":" + remainingSeconds.ToString("00");
    }
}
