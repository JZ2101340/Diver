using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform contentParent;
    private string leaderboardUrl = "http://localhost:3000/leaderboard";

    [System.Serializable]
    public class LeaderboardData
    {
        public string username;
        public int total_score;
        public int time_taken;
        public int rank;
    }

    public void Start()
    {
        StartCoroutine(LoadLeaderboard());
    }

    IEnumerator LoadLeaderboard()
    {
        UnityWebRequest www = UnityWebRequest.Get(leaderboardUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch leaderboard: " + www.error);
            yield break;
        }

        LeaderboardData[] data = JsonHelper.FromJson<LeaderboardData>(www.downloadHandler.text);

        foreach (LeaderboardData entry in data)
        {
            GameObject go = Instantiate(entryPrefab, contentParent);
            go.GetComponent<LeaderboardEntry>().Initialize(entry.rank, entry.username, entry.total_score, entry.time_taken);
        }
    }
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }

}
