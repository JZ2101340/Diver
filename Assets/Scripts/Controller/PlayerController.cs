using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Camera mainCamera;
    public Joystick joystick;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public AudioSource hitSound;
    public AudioSource boostSound;
    public AudioSource checkpointSound;
    public float cameraFollowSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        movement = new Vector2(moveX, moveY).normalized * speed;

        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (moveX < 0)
            spriteRenderer.flipX = true;
        else if (moveX > 0)
            spriteRenderer.flipX = false;

        if (mainCamera != null)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movement;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with: " + other.gameObject.name);

        if (other.CompareTag("Checkpoint"))
        {
            FindObjectOfType<CheckpointManager>().GetReachedCheckpointCount();
            other.gameObject.SetActive(false);  

            if (checkpointSound != null) checkpointSound.Play();
        }
        if (other.CompareTag("SeaCreature"))
        {
            FindObjectOfType<OxygenManager>().DecreaseOxygen(20f);
            Debug.Log("Hit a creature! Oxygen reduced.");

            if (hitSound != null) hitSound.Play();
        }
        if (other.CompareTag("Booster"))
        {
            FindObjectOfType<OxygenManager>().IncreaseOxygen(30f);
            Destroy(other.gameObject);

            if (boostSound != null) boostSound.Play();
        }
    }
}
