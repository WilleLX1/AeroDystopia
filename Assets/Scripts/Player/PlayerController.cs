using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float crouchSpeed = 3f; 
    public float stamina = 100f;
    public float staminaDepletionRate = 20;
    public float staminaRecoveryRate = 10;
    public bool isSprinting = false;
    public bool canMove = true;
    public float rotationSpeed = 700f;
    public float jumpForce = 8f;
    private Vector3 velocity;
    private bool isJumping;
    public float gravity = 30f;
    public float mouseSensitivity = 100f;

    public float interactionDistance = 5.0f; // Adjust this value to change the interaction distance

    public float normalHeight = 2f;
    public float crouchHeight = 1f;

    public GameObject cameraObject;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private float xRotation = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (canMove){
            HandleMovement();    
            HandleMouseLook();

            // Check if the jump button is pressed
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);
                isJumping = true;
            }
            // Check if the crouch key is pressed
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Vector3 position = cameraObject.transform.position;
                position.y -= 1; // Move the camera down by 1 unit
                cameraObject.transform.position = position;
                speed = crouchSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Vector3 position = cameraObject.transform.position;
                position.y += 1; // Move the camera up by 1 unit
                cameraObject.transform.position = position;
                speed = walkSpeed;
            }
            // Check if the sprint key is pressed
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            {
                // Run
                speed = runSpeed;
                stamina -= Time.deltaTime * staminaDepletionRate; // Remove the cast to int
                isSprinting = true;
            }
            else
            {
                // Walk
                speed = walkSpeed;
                isSprinting = false;
            }

            // Apply the velocity
            characterController.Move(velocity * Time.deltaTime);

        }
        

        // Apply gravity
        velocity.y -= gravity * Time.deltaTime;

        // Check if the interact key is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
        

        // Recover stamina when not sprinting
        if (!isSprinting && stamina < 100)
        {
            stamina += Time.deltaTime * staminaRecoveryRate; // Remove the cast to int
        }

        // If the character is grounded, they are not jumping
        if (characterController.isGrounded)
        {
            isJumping = false;
            velocity.y = 0;
        }
    }

    void Interact()
    {
        // Cast a ray forward from the player
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // If the ray hits an object
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Check if the object has an Interactable component
            Interactable interactable = hit.transform.GetComponent<Interactable>();
            if (interactable != null)
            {
                // Interact with the object
                interactable.Interact();
            }
        }
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the direction to move based on the camera's rotation
        Vector3 forward = cameraObject.transform.forward;
        Vector3 right = cameraObject.transform.right;

        // Normalize vectors to ensure consistent movement speed
        forward.y = 0;
        forward.Normalize();
        right.y = 0;
        right.Normalize();

        Vector3 desiredMoveDirection = forward * verticalInput + right * horizontalInput;
        desiredMoveDirection.Normalize();

        // Move the character in the direction the camera is facing
        characterController.Move(desiredMoveDirection * speed * Time.deltaTime);

        // Jump
        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            moveDirection.y = jumpForce;
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the rotation to the camera directly
        cameraObject.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            
        // Rotate the entire player object (including the camera) around the Y-axis
        transform.Rotate(Vector3.up * mouseX);
    }
    
    void OnTriggerEnter(Collider other)
    {
        
    }

    void OnTriggerExit(Collider other)
    {
        
    }

}
