using UnityEngine;

public class Booster : MonoBehaviour
{
    public float oxygenAmount = 100f; // model
    private void OnTriggerEnter2D(Collider2D other) //controller
    {
        if (other.CompareTag("Player"))
        {
            OxygenManager oxygenManager = FindObjectOfType<OxygenManager>();
            if (oxygenManager != null)
            {
                oxygenManager.IncreaseOxygen(oxygenAmount);
                Destroy(gameObject); 
            }
        }
    }
}
// for booster view is the UI booster itself
