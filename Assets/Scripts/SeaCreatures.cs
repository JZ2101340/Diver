using UnityEngine;

public class SeaCreature : MonoBehaviour
{
    public float oxygenReduction = 20f; // Oxygen lost when player touches
    public float moveSpeed = 2f; // Speed of sea creature movement
    public Vector2 movementDirection; // Random movement direction
    public float changeDirectionInterval = 3f; // How often it changes direction

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Prevent gravity affecting movement
        InvokeRepeating("ChangeDirection", 0f, changeDirectionInterval);
    }

    void Update()
    {
        rb.velocity = movementDirection * moveSpeed; // Apply movement
    }

    private void ChangeDirection()
    {
        movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OxygenManager oxygenManager = FindObjectOfType<OxygenManager>();
            if (oxygenManager != null)
            {
                oxygenManager.DecreaseOxygen(oxygenReduction);
            }
        }
    }
}
