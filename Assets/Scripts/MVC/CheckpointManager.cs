using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CheckpointManager : MonoBehaviour
{
    public Transform[] checkpoints;            
    public Transform player;                   
    public RectTransform compassArrow;         
    public TMP_Text checkpointText;            
    public int totalCheckpoints;               
    private int reachedCheckpoints = 0;
    private int currentCheckpointIndex = 0;

    public static CheckpointManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


  
    void Start()
    {
        if (PlayerPrefs.GetInt("restoreProgress", 0) == 1)
        {
            int checkpointIndex = PlayerPrefs.GetInt("checkpointsReached", 0);
            reachedCheckpoints = checkpointIndex;
            currentCheckpointIndex = checkpointIndex;
            Debug.Log("? Checkpoints Restored: " + reachedCheckpoints);
        }

        ActivateCheckpoint(currentCheckpointIndex);
        UpdateCheckpointUI();
    }



    void Update()
    {
        UpdateCompass();
    }

    public void RegisterCheckpointHit()
    {
        reachedCheckpoints++;

        currentCheckpointIndex++;
        UpdateCheckpointUI();

        if (reachedCheckpoints >= totalCheckpoints)
        {
            LoadNextScene();
        }
        else
        {
            ActivateCheckpoint(currentCheckpointIndex);
        }
    }

    void UpdateCheckpointUI()
    {
        if (checkpointText != null)
        {
            checkpointText.text = $"Checkpoints Reached:\n{reachedCheckpoints}/{totalCheckpoints}";
        }
    }

    void ActivateCheckpoint(int index)
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].gameObject.SetActive(i == index); 
        }
    }

    void UpdateCompass()
    {
        if (compassArrow == null || player == null || currentCheckpointIndex >= checkpoints.Length)
            return;

        Vector3 direction = (checkpoints[currentCheckpointIndex].position - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        compassArrow.localRotation = Quaternion.Euler(0, 0, angle - 90f);

    }



    void UploadFinalScoreAndLoadScene(int nextSceneIndex)
    {
        int userId = int.Parse(NetworkManager.userId);
        int finalScore = ScoreManager.Instance.GetScore();
        int timeTakenInSeconds = Mathf.RoundToInt(TimerManager.Instance.GetTime());

        Debug.Log($"Uploading Final Score: {finalScore}, Time: {timeTakenInSeconds}s");

        LeaderboardUploader uploader = FindObjectOfType<LeaderboardUploader>();
        if (uploader != null)
        {
            StartCoroutine(UploadAndLoad(uploader, userId, finalScore, timeTakenInSeconds, nextSceneIndex));
        }
        else
        {
            Debug.LogError("LeaderboardUploader not found!");
        }
    }
    IEnumerator UploadAndLoad(LeaderboardUploader uploader, int userId, int finalScore, int timeTaken, int nextSceneIndex)
    {
        yield return uploader.UploadScore(userId, finalScore, timeTaken);

        SceneManager.LoadScene(nextSceneIndex);
    }


    void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            UploadFinalScoreAndLoadScene(nextSceneIndex);
        }
        
    }



    public void RestoreCheckpoint(int index)
    {
        currentCheckpointIndex = index;
        reachedCheckpoints = index;
        ActivateCheckpoint(currentCheckpointIndex);
        UpdateCheckpointUI();
    }

    public int GetReachedCheckpointCount()
    {
        return reachedCheckpoints;
    }
}
