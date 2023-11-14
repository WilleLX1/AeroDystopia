using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public GameObject player; // Drag your player object here in the inspector
    public GameObject playerParent; // Drag the desired parent object here in the inspector

    private Vector3 originalPosition;
    private float time;
    public float speed = 1.0f; // Adjust this value to make the spaceship move faster or slower
    public float amplitude = 15.0f; // Adjust this value to make the spaceship move a greater or lesser distance
    private Collider playerCollider;
    private Rigidbody playerRigidbody;
    
    void Start()
    {
        // Store the original position
        originalPosition = transform.position;

        // Get the player's collider
        if (player != null)
        {
            playerCollider = player.GetComponent<Collider>();
            playerRigidbody = player.GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // Calculate the new position
        Vector3 newPosition = originalPosition + new Vector3(
            Mathf.Sin(time * speed) * amplitude, // X position
            Mathf.Sin(time * speed) * amplitude, // Y position
            Mathf.Sin(time * speed) * amplitude  // Z position
        );

        // Update the spaceship's position
        transform.position = newPosition;

        // Increment the time
        time += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the triggering object is the player
        if (other.gameObject == player)
        {
            // Make the player a child of the specified parent
            player.transform.SetParent(playerParent.transform);

            // Disable the player's Rigidbody
            if (playerRigidbody != null)
            {
                playerRigidbody.isKinematic = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the triggering object is the player
        if (other.gameObject == player)
        {
            // Unparent the player
            player.transform.SetParent(null);

            // Enable the player's Rigidbody
            if (playerRigidbody != null)
            {
                playerRigidbody.isKinematic = false;
            }
        }
    }
}