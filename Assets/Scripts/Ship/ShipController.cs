using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 3f;
    public float verticalSpeed = 3f;
    public bool canMove = false;

    void Update()
    {
        if (canMove){
            // Forward and backward movement
            float verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector3.forward * verticalInput * speed * Time.deltaTime);

            // Left and right rotation
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);

            // Upward movement with Left Shift
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime);
            }

            // Downward movement with Left CTRL
            if (Input.GetKey(KeyCode.LeftControl))
            {
                transform.Translate(Vector3.down * verticalSpeed * Time.deltaTime);
            }
        }
    }
}
