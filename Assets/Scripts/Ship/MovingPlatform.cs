using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject player; // Drag and drop the player GameObject into this field in the Inspector
    public float speed = 5f; // Adjust the speed of the platform as needed
    public bool PlayerControls = false;
    public float rotationSpeed = 45f;  // Adjust the rotation speed as needed
    public string followerTag = "Pickup"; // Set the tag for objects that should follow the platform
    
    [Header("Auto move")]
    public float distance = 10f; // Adjust the total distance the platform should move
    public bool AutoMove = true; // Set to true if the platform should move automatically
    public Vector3[] points;
    private int targetPointIndex = 0;
    
    [Header("Ship Directions")]
    public bool isCurrentlyMovingForward = false;
    public bool isCurrentlyMovingBackwards = false;
    public bool isCurrentlyMovingRight = false;
    public bool isCurrentlyMovingLeft = false;
    private bool playerOnPlatform = false;
    private Vector3 playerOffset;
    private bool movingForward = true;
    private Vector3 startPosition;
    private float rotationDirection = 1f; // 1 for clockwise, -1 for counter-clockwise

    private void Start()
    {
        startPosition = transform.position;
        points = new Vector3[4];
        points[0] = startPosition;
        points[1] = startPosition + new Vector3(10, 0, 0); // Adjust these values to define the rectangle
        points[2] = startPosition + new Vector3(10, 0, 10);
        points[3] = startPosition + new Vector3(0, 0, 10);
    }

    private void FixedUpdate()
    {
        float movement = 0f;
        float horizontalInput = Input.GetAxis("Horizontal");

        if (PlayerControls)
        {
            HandlePlayerInput(ref movement);
            transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        }
        else if (AutoMove)
        {
            Vector3 targetPoint = points[targetPointIndex];
            Vector3 direction = (targetPoint - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationDirection * rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                targetPointIndex = (targetPointIndex + 1) % points.Length;
                rotationDirection = -rotationDirection; // Change rotation direction at each corner
            }
        }

        // If the player is on the platform, move the player with the platform while maintaining the offset
        if (playerOnPlatform)
        {
            //MoveObjectWithPlatform(player.transform);
        }

        MoveObjectsWithPlatformTagged(followerTag);

        transform.Translate(Vector3.forward * movement);

        if (AutoMove && Vector3.Distance(startPosition, transform.position) >= distance)
        {
            // Check if the platform has reached the end and needs to reverse direction
            movingForward = !movingForward;
        }

    }
    private void HandlePlayerInput(ref float movement)
    {
        movement = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            movement = speed * Time.deltaTime;
            SetMovementFlags(false, false, false, true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement = -speed * Time.deltaTime;
            SetMovementFlags(false, false, true, false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movement = -speed * Time.deltaTime;
            SetMovementFlags(true, false, false, false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement = speed * Time.deltaTime;
            SetMovementFlags(false, true, false, false);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += Vector3.up * speed * Time.deltaTime; // Move up
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.position += Vector3.down * speed * Time.deltaTime; // Move down
        }
        else
        {
            SetMovementFlags(false, false, false, false);
        }
    }

    private void SetMovementFlags(bool left, bool right, bool backward, bool forward)
    {
        isCurrentlyMovingLeft = left;
        isCurrentlyMovingRight = right;
        isCurrentlyMovingBackwards = backward;
        isCurrentlyMovingForward = forward;
    }
    private void MoveObjectsWithPlatformTagged(string tag)
    {
        Collider platformCollider = GetComponent<Collider>();
        if (platformCollider != null)
        {
            GameObject[] followers = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject follower in followers)
            {
                Collider followerCollider = follower.GetComponent<Collider>();
                if (followerCollider != null && followerCollider.bounds.Intersects(platformCollider.bounds))
                {
                    // Parent the object to the platform to move together
                    follower.transform.parent = transform;
                    //MoveObjectWithPlatform(follower.transform);
                }
            }
        }
    }
    
    private void MoveObjectWithPlatform(Transform objTransform)
    {
        Vector3 objectMovement = Vector3.zero;

        if (isCurrentlyMovingForward)
        {
            objectMovement = Vector3.forward * speed * Time.deltaTime;
        }
        else if (isCurrentlyMovingBackwards)
        {
            objectMovement = Vector3.back * speed * Time.deltaTime;
        }
        else if (isCurrentlyMovingRight)
        {
            objectMovement = Vector3.right * speed * Time.deltaTime;
        }
        else if (isCurrentlyMovingLeft)
        {
            objectMovement = Vector3.left * speed * Time.deltaTime;
        }

        objTransform.position += objectMovement;
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player or an object with the specified tag
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag(followerTag))
        {
            // Parent the object to the platform to move together
            other.transform.parent = transform;

            // If the colliding object is the player, store the offset between the player and the platform
            if (other.gameObject.CompareTag("Player"))
            {
                playerOnPlatform = true;
                playerOffset = other.transform.position - transform.position;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the colliding object is the player or an object with the specified tag
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag(followerTag))
        {
            // Unparent the object to stop moving with the platform
            other.transform.parent = null;

            // If the colliding object is the player, update the flag
            if (other.gameObject.CompareTag("Player"))
            {
                playerOnPlatform = false;
            }
        }
    }
}
