////using UnityEngine;

////public class PlayerController : MonoBehaviour
////{
////    public float speed = 5f;
////    private Rigidbody2D rb;
////    private Vector2 movement;
////    private Camera mainCamera;

////    // References
////    public Joystick joystick;
////    private Animator animator;
////    private SpriteRenderer spriteRenderer; // For flipping the sprite

////    void Start()
////    {
////        rb = GetComponent<Rigidbody2D>();
////        mainCamera = Camera.main;
////        animator = GetComponent<Animator>();
////        spriteRenderer = GetComponent<SpriteRenderer>();
////    }

////    void Update()
////    {
////        // Get movement input from the joystick
////        float moveX = joystick.Horizontal;
////        float moveY = joystick.Vertical;

////        // Store movement
////        movement = new Vector2(moveX, moveY).normalized * speed;

////        // **Update Animator Speed Parameter**
////        animator.SetFloat("Speed", movement.sqrMagnitude);

////        // **Flip sprite when moving left**
////        if (moveX < 0)
////            spriteRenderer.flipX = true;
////        else if (moveX > 0)
////            spriteRenderer.flipX = false;
////    }

////    void FixedUpdate()
////    {
////        rb.velocity = movement;
////        KeepDiverInsideScreen();
////    }

////    void OnTriggerEnter2D(Collider2D other)
////    {
////        Debug.Log("Collided with: " + other.gameObject.name); // Debug collision  

////        if (other.CompareTag("Checkpoint"))
////        {
////            FindObjectOfType<CheckpointManager>().CheckpointReached();
////            Destroy(other.gameObject);
////        }
////        if (other.CompareTag("SeaCreature"))
////        {
////            FindObjectOfType<OxygenManager>().DecreaseOxygen(20f);
////            Debug.Log("Hit a creature! Oxygen reduced.");
////        }
////        if (other.CompareTag("Booster"))
////        {
////            FindObjectOfType<OxygenManager>().IncreaseOxygen(30f);
////            Destroy(other.gameObject);
////        }
////    }

////    void KeepDiverInsideScreen()
////    {
////        if (mainCamera == null) return;

////        float cameraHeight = mainCamera.orthographicSize;
////        float cameraWidth = cameraHeight * mainCamera.aspect;

////        float playerWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
////        float playerHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

////        float minX = -cameraWidth + playerWidth;
////        float maxX = cameraWidth - playerWidth;
////        float minY = -cameraHeight + playerHeight;
////        float maxY = cameraHeight - playerHeight;

////        Vector3 clampedPosition = transform.position;
////        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
////        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
////        transform.position = clampedPosition;
////    }
////}

//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    public float speed = 5f;
//    private Rigidbody2D rb;
//    private Vector2 movement;
//    private Camera mainCamera;

//    // References
//    public Joystick joystick;
//    private Animator animator;
//    private SpriteRenderer spriteRenderer;

//    // ?? Sound Effects
//    public AudioSource hitSound;       // When touching a sea creature
//    public AudioSource boostSound;     // When collecting a booster
//    public AudioSource checkpointSound; // When reaching a checkpoint

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        mainCamera = Camera.main;
//        animator = GetComponent<Animator>();
//        spriteRenderer = GetComponent<SpriteRenderer>();
//    }

//    void Update()
//    {
//        // Get movement input from the joystick
//        float moveX = joystick.Horizontal;
//        float moveY = joystick.Vertical;

//        // Store movement
//        movement = new Vector2(moveX, moveY).normalized * speed;

//        // **Update Animator Speed Parameter**
//        animator.SetFloat("Speed", movement.sqrMagnitude);

//        // **Flip sprite when moving left**
//        if (moveX < 0)
//            spriteRenderer.flipX = true;
//        else if (moveX > 0)
//            spriteRenderer.flipX = false;
//    }

//    void FixedUpdate()
//    {
//        rb.velocity = movement;
//        //KeepDiverInsideScreen();
//    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        Debug.Log("Collided with: " + other.gameObject.name); // Debug collision  

//        if (other.CompareTag("Checkpoint"))
//        {
//            FindObjectOfType<CheckpointManager>().CheckpointReached();
//            Destroy(other.gameObject);

//            // ?? Play Checkpoint Sound
//            if (checkpointSound != null) checkpointSound.Play();
//        }
//        if (other.CompareTag("SeaCreature"))
//        {
//            FindObjectOfType<OxygenManager>().DecreaseOxygen(20f);
//            Debug.Log("Hit a creature! Oxygen reduced.");

//            // ?? Play Hit Sound
//            if (hitSound != null) hitSound.Play();
//        }
//        if (other.CompareTag("Booster"))
//        {
//            FindObjectOfType<OxygenManager>().IncreaseOxygen(30f);
//            Destroy(other.gameObject);

//            // ?? Play Boost Sound
//            if (boostSound != null) boostSound.Play();
//        }
//    }

//    //void KeepDiverInsideScreen()
//    //{
//    //    if (mainCamera == null) return;

//    //    float cameraHeight = mainCamera.orthographicSize;
//    //    float cameraWidth = cameraHeight * mainCamera.aspect;

//    //    float playerWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
//    //    float playerHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

//    //    float minX = -cameraWidth + playerWidth;
//    //    float maxX = cameraWidth - playerWidth;
//    //    float minY = -cameraHeight + playerHeight;
//    //    float maxY = cameraHeight - playerHeight;

//    //    Vector3 clampedPosition = transform.position;
//    //    clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
//    //    clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
//    //    transform.position = clampedPosition;
//    //}
//}
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Camera mainCamera;

    // References
    public Joystick joystick;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Sound Effects
    public AudioSource hitSound;
    public AudioSource boostSound;
    public AudioSource checkpointSound;

    // Camera Follow
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
        // Get movement input from the joystick
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        // Store movement
        movement = new Vector2(moveX, moveY).normalized * speed;

        // Update Animator Speed Parameter
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Flip sprite when moving left
        if (moveX < 0)
            spriteRenderer.flipX = true;
        else if (moveX > 0)
            spriteRenderer.flipX = false;

        // Camera Follow Logic
        if (mainCamera != null)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movement;
        //KeepDiverInsideScreen();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with: " + other.gameObject.name);

        if (other.CompareTag("Checkpoint"))
        {
            //Destroy(other.gameObject);
            FindObjectOfType<CheckpointManager>().GetReachedCheckpointCount();
            other.gameObject.SetActive(false);  // ? safer than destroy

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
