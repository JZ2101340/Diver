using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public Transform[] checkpoints;  // Assign in Inspector
    private int currentCheckpointIndex = 0;

    public RectTransform compassArrow;
    public Transform player;

    void Start()
    {
        // Initialize the first checkpoint
        ActivateCheckpoint(0);
    }

    void Update()
    {
        // If we haven't reached all checkpoints, update the compass
        if (currentCheckpointIndex < checkpoints.Length)
        {
            UpdateCompass();
        }
    }

    // Activates the specified checkpoint
    void ActivateCheckpoint(int index)
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            // Only the current checkpoint is active
            checkpoints[i].gameObject.SetActive(i == index);
        }
    }

    // Updates the compass to point towards the next checkpoint
    void UpdateCompass()
    {
        if (compassArrow != null && player != null)
        {
            // Direction from the player to the current checkpoint
            Vector2 direction = checkpoints[currentCheckpointIndex].position - player.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Smooth rotation of the compass arrow
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            compassArrow.rotation = Quaternion.Lerp(compassArrow.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    // Called when the player reaches a checkpoint
    public void CheckpointReached()
    {
        currentCheckpointIndex++;

        // If there are more checkpoints, activate the next one
        if (currentCheckpointIndex < checkpoints.Length)
        {
            ActivateCheckpoint(currentCheckpointIndex);
        }
        else
        {
            // If all checkpoints are reached, the level is completed
            Debug.Log("Level Completed!");
            // Add any level transition logic here, such as loading the next scene
            SceneManager.LoadScene("NextLevelScene");  // Replace with the correct next level name
        }
    }
}
