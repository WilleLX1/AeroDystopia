using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDoorController : MonoBehaviour
{
    public bool isOpen = false;
    public float doorSpeed = 2f;
    public float hydralicPowerLeft = 1000f;
    public GameObject openPosition;
    public GameObject closedPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    async void Update()
    {
        if (!isOpen && transform.position != closedPosition.transform.position)
        {
            // Move towards the closed position
            transform.position = Vector3.MoveTowards(transform.position, closedPosition.transform.position, doorSpeed * Time.deltaTime);

            // Shrink the door while it's moving up
            if (transform.localScale.y > 0.1f) // Prevent the door from disappearing completely
            {
                transform.localScale = new Vector3(3, 3.5f, 0.2f); // Adjust the shrinking speed as needed
            }
        }
        else if (isOpen && transform.position != openPosition.transform.position)
        {
            // Move towards the open position and restore the original scale
            transform.position = Vector3.MoveTowards(transform.position, openPosition.transform.position, doorSpeed * Time.deltaTime);
            transform.localScale = new Vector3(3, 1.65f, 0.2f); // Adjust the shrinking speed as needed
        }

        if (!isOpen){
            // Make hydralicPowerLeft go down whilst door is open, and regenerate when closed
            if (hydralicPowerLeft > 0f){
                hydralicPowerLeft -= 0.05f;
            }
            else if (hydralicPowerLeft < 0f) {
                isOpen = true;
            }
        }
        else {
            if (hydralicPowerLeft < 1000f){
                hydralicPowerLeft += 0.1f;
            }
        }
    }
}
