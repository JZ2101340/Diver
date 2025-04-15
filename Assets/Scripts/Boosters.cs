using UnityEngine;

public class Booster : MonoBehaviour
{
    public float oxygenAmount = 100f; // How much oxygen this gives

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OxygenManager oxygenManager = FindObjectOfType<OxygenManager>();
            if (oxygenManager != null)
            {
                oxygenManager.IncreaseOxygen(oxygenAmount);
                Destroy(gameObject); // Remove booster after pickup
            }
        }
    }
}
