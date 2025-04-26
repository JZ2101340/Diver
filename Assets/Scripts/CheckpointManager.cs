using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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



    void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // ? Reset score and timer for the next level
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.shouldReset = true;

            if (TimerManager.Instance != null)
                TimerManager.Instance.shouldReset = true;

            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("? All levels complete.");
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
