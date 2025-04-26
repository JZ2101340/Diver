using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;

            if (CheckpointManager.Instance != null)
            {
                CheckpointManager.Instance.RegisterCheckpointHit();
            }

            gameObject.SetActive(false);
        }
    }
}
