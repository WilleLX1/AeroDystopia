using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Interactable : MonoBehaviour
{
    [Header("Items (Pickups)")]
    public bool isHoldingItem = false;

    [Header("Ship Control")]
    public bool isControllingShip = false;
    public GameObject ship;
    public Camera shipCamera;
    public float shipSpeed = 5f;
    public float shipRotationSpeed = 5f;
    
    private Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started interactable with " + transform.name);

        // Check if object has a box collider
        if (GetComponent<BoxCollider>() == null)
        {
            Debug.LogWarning("Interactable object " + transform.name + " does not have a box collider.");
        }
        else
        {
            collider = GetComponent<BoxCollider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoldingItem)
        {
            // Move the object to the player's hands
            Transform hands = GameObject.Find("Player/Hands").transform;
            transform.SetParent(hands);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }

    public void Interact()
    {
        Debug.Log("Interacting with " + transform.name);

        // If tag on current gameobject is "Pickup" then move it to the player hands
        if (gameObject.CompareTag("Pickup"))
        {
            if (!isHoldingItem)
            {
                // Move the object to the player's hands
                Transform hands = GameObject.Find("Player/Hands").transform;
                transform.SetParent(hands);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
                isHoldingItem = true;
                // Deactivate collider
                collider.enabled = false;
            }
            else if (isHoldingItem)
            {
                // Activate collider
                collider.enabled = true;
                // Raycast to find the ground
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
                {
                    isHoldingItem = false;
                    // Move the object to the ground position
                    transform.SetParent(null);
                    // Place object on hit.point but move it 1 unit above the ground
                    hit.point += Vector3.up - new Vector3(0, 0.47f, 0);
                    transform.position = hit.point;
                    transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
                else
                {
                    Debug.LogWarning("Couldn't find the ground. Object not released.");
                }
            }
        }
        else if (gameObject.CompareTag("Control"))
        {
            // Check if already controlling ship or not
            if (isControllingShip)
            {
                Debug.Log("Stopped controlling ship");
                // Stop controlling ship
                isControllingShip = false;
                // Disable ship camera
                shipCamera.enabled = false;
            }
            else if (!isControllingShip)
            {
                Debug.Log("Started controlling ship");
                // Start controlling ship
                isControllingShip = true;
                // Enable ship camera
                shipCamera.enabled = true;

                // Get the horizontal and vertical axis values
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                // Move the ship forward and backward
                ship.transform.Translate(Vector3.forward * vertical * shipSpeed * Time.deltaTime);

                // Rotate the ship left and right
                ship.transform.Rotate(Vector3.up, horizontal * shipRotationSpeed * Time.deltaTime);
            }
        }
    }
}

