using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    public Transform leaderboardContent; // The content object of the scroll view (where the leaderboard entries will go)
    public GameObject leaderboardEntryPrefab; // Prefab for a leaderboard entry

    void Start()
    {
        StartCoroutine(FetchLeaderboard());
    }

    IEnumerator FetchLeaderboard()
    {
        string url = "http://localhost:3000/leaderboard"; // Update to your actual server URL

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                List<PlayerData> leaderboard = JsonUtilityWrapper.FromJsonArray<PlayerData>(json);
                DisplayLeaderboard(leaderboard);
            }
            
        }
    }

    void DisplayLeaderboard(List<PlayerData> leaderboard)
    {
        // Clear the existing leaderboard entries
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        // Populate the leaderboard with new entries
        for (int i = 0; i < leaderboard.Count; i++)
        {
            GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContent);
            LeaderboardEntry entryScript = entry.GetComponent<LeaderboardEntry>();

            entryScript.SetRank(i + 1);
            entryScript.SetName(leaderboard[i].username);
            entryScript.SetScore(leaderboard[i].score);
            entryScript.SetTime(leaderboard[i].time); // Ensure you send time from the server
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string username;
        public int score;
        public float time; // Assuming time is in seconds
    }
}

// Helper for parsing JSON arrays
public static class JsonUtilityWrapper
{
    public static List<T> FromJsonArray<T>(string json)
    {
        string wrappedJson = "{\"items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrappedJson);
        return new List<T>(wrapper.items);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}
