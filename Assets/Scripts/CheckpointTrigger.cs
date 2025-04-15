using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointTrigger : MonoBehaviour
{
    public TMP_Text checkpointText; // Assigned in Inspector
    public static int totalCheckpoints = 2; // Total number of checkpoints
    public static int checkpointsReached = 0;
    public static bool sceneLoaded = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        // When the player collides with a checkpoint
        if (other.CompareTag("Player") && !sceneLoaded)
        {
            checkpointsReached++;
            gameObject.SetActive(false); // Disable this checkpoint after reaching it

            // Update the checkpoint UI if it’s assigned
            if (checkpointText != null)
            {
                checkpointText.text = $"Checkpoints Reached: {checkpointsReached}/{totalCheckpoints}";
            }

            // If all checkpoints are reached, load the next level
            if (checkpointsReached >= totalCheckpoints)
            {
                sceneLoaded = true;
                Debug.Log("Game won!");
                SceneManager.LoadScene("Medium"); // Replace with the correct scene name for the next level
            }
        }
    }
}
