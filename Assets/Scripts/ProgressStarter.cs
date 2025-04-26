using UnityEngine;
using System.Collections;

public class ProgressStarter : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f); // let the scene load

        while (OxygenManager.Instance == null || ScoreManager.Instance == null || CheckpointManager.Instance == null)
        {
            Debug.Log("? Waiting for managers to initialize...");
            yield return new WaitForSeconds(0.5f);
        }

        if (NetworkManager.Instance != null)
        {
            Debug.Log("? ProgressStarter: All managers found. Starting live progress saving...");
            NetworkManager.Instance.StartProgressSaving();
        }
        else
        {
            Debug.LogWarning("? ProgressStarter: NetworkManager not found.");
        }
    }
}
