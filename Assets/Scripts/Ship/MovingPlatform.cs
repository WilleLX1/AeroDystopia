using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject player; // Drag and drop the player GameObject into this field in the Inspector
    public float speed = 5f; // Adjust the speed of the platform as needed
    public float distance = 10f; // Adjust the total distance the platform should move
    public string followerTag = "Pickup"; // Set the tag for objects that should follow the platform

    private bool playerOnPlatform = false;
    private Vector3 playerOffset;
    private bool movingForward = true;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Calculate the movement of the platform
        float movement = speed * (movingForward ? 1 : -1) * Time.deltaTime;

        // If the player is on the platform, move the player with the platform while maintaining the offset
        if (playerOnPlatform)
        {
            MoveObjectWithPlatform(player.transform);
        }

        // Move other objects with the specified tag that are within the platform's collider
        Collider platformCollider = GetComponent<Collider>();
        if (platformCollider != null)
        {
            GameObject[] followers = GameObject.FindGameObjectsWithTag(followerTag);
            foreach (GameObject follower in followers)
            {
                Collider followerCollider = follower.GetComponent<Collider>();
                if (followerCollider != null && followerCollider.bounds.Intersects(platformCollider.bounds))
                {
                    MoveObjectWithPlatform(follower.transform);
                }
            }
        }

        // Move the platform
        transform.Translate(Vector3.forward * movement);

        // Check if the platform has reached the end and needs to reverse direction
        if (Vector3.Distance(startPosition, transform.position) >= distance)
        {
            movingForward = !movingForward;
        }
    }
    
    private void MoveObjectWithPlatform(Transform objTransform)
    {
        // Calculate the relative movement for the object
        Vector3 objectMovement = Vector3.forward * speed * (movingForward ? 1 : -1) * Time.deltaTime;

        // Move the object with the relative movement
        objTransform.Translate(objectMovement, Space.World);
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player or an object with the specified tag
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag(followerTag))
        {
            // Parent the object to the platform to move together
            //other.transform.parent = transform;

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
            //other.transform.parent = null;

            // If the colliding object is the player, update the flag
            if (other.gameObject.CompareTag("Player"))
            {
                playerOnPlatform = false;
            }
        }
    }
}
