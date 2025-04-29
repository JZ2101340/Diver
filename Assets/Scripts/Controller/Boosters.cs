using UnityEngine;

public class Booster : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OxygenManager oxygenManager = FindObjectOfType<OxygenManager>();
            if (oxygenManager != null)
            {
                oxygenManager.SetOxygenToMax(); 
                Destroy(gameObject);
            }
        }
    }
}
