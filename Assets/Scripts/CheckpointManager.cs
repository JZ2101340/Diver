using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CheckpointManager : MonoBehaviour
{

    [Header("Setup")]
    public Transform[] checkpoints;            // ?? Assign all checkpoint GameObjects
    public Transform player;                   // ?? Assign the Diver
    public RectTransform compassArrow;         // ?? Assign the UI compass arrow
    public TMP_Text checkpointText;            // ?? Assign the UI text

    [Header("Checkpoint Settings")]
    public int totalCheckpoints;               // ?? Set manually (or use checkpoints.Length)

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
            checkpoints[i].gameObject.SetActive(i == index); // only next one active
        }
    }

    void UpdateCompass()
    {
        if (compassArrow == null || player == null || currentCheckpointIndex >= checkpoints.Length)
            return;

        // Get direction in world space
        Vector3 direction = checkpoints[currentCheckpointIndex].position - player.position;

        // Convert to angle in Z (2D)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation on Z axis only
        compassArrow.localRotation = Quaternion.Euler(0, 0, angle);
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

        // After upload finishes, load the next scene
        SceneManager.LoadScene(nextSceneIndex);
    }


    void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            UploadFinalScoreAndLoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("? All levels complete.");
            // You can load a "Game Completed" scene if you want
            // SceneManager.LoadScene("LeaderboardScene");
        }
    }



    public void RestoreCheckpoint(int index)
    {
        currentCheckpointIndex = index;
        reachedCheckpoints = index;
        ActivateCheckpoint(currentCheckpointIndex);
        UpdateCheckpointUI();
    }

    // Optional for other scripts
    public int GetReachedCheckpointCount()
    {
        return reachedCheckpoints;
    }
}
