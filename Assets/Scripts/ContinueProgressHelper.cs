using UnityEngine;

public class ContinueProgressHelper : MonoBehaviour
{
    public void OnContinueButtonClicked()
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.ContinueProgress();
        }
    }
}
