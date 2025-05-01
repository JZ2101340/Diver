//using UnityEngine;

//public class SeaCreature : MonoBehaviour
//{
//    public float oxygenReduction = 5f;
//    public float moveSpeed = 2f;
//    public Vector2 movementDirection;
//    public float changeDirectionInterval = 3f;

//    private Rigidbody2D rb;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        rb.gravityScale = 0;
//        InvokeRepeating("ChangeDirection", 0f, changeDirectionInterval);
//    }

//    void Update()
//    {
//        rb.velocity = movementDirection * moveSpeed;
//    }

//    private void ChangeDirection()
//    {
//        movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            OxygenManager oxygenManager = FindObjectOfType<OxygenManager>();
//            if (oxygenManager != null)
//            {
//                oxygenManager.DecreaseOxygen(oxygenReduction);
//            }
//        }
//    }

//}

using UnityEngine;

public class SeaCreature : MonoBehaviour
{
    public float oxygenReduction = 5f;
    public float moveSpeed = 2f;
    public Vector2 movementDirection;
    public float changeDirectionInterval = 3f;

    private Rigidbody2D rb;
    private bool hasHitPlayer = false;
    private float hitCooldown = 1f;
    private float lastHitTime = -10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        InvokeRepeating("ChangeDirection", 0f, changeDirectionInterval);
    }

    void Update()
    {
        rb.velocity = movementDirection * moveSpeed;
    }

    private void ChangeDirection()
    {
        movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Prevent multiple hits in a short time
        if (Time.time - lastHitTime < hitCooldown) return;

        lastHitTime = Time.time;

        OxygenManager oxygenManager = FindObjectOfType<OxygenManager>();
        if (oxygenManager != null)
        {
            Debug.Log("SeaCreature hit! Reducing oxygen by 5.");
            oxygenManager.DecreaseOxygen(oxygenReduction);
        }
    }
}

