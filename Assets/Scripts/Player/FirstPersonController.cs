using UnityEngine;


using Unity.Cinemachine;
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;            
    [SerializeField] private float sprintMultiplier = 2.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravityMultiplier = 1.0f;

    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.01f;
    [SerializeField] private float upDownLookRange = 80f;               // Clamp for vertical camera rotation

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 1.0f;

    [Header("References")]
    [SerializeField] private CharacterController characterController;   // Handling collision and movement
    [SerializeField] private CinemachineCamera playerCam;               // PLayer camera (child of player)
    [SerializeField] private PlayerInputHandler playerInputHandler;     // Handles input system values

    private Vector3 currentMovement;                                    // Stores x, y, z movement velocity
    private float verticalRotation;                                     // Tracks camera up and down rotation
    // Calculates current movement speed with sprinting active or not
    private float CurrentSpeed => walkSpeed * (playerInputHandler.SprintTriggered ? sprintMultiplier : 1);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // CURSOR
        // Locks cursor to center of screen and hides it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement(); 
        HandleRotation();
    }

    /*
    Converts WASD input into a world direction based on the rotation of the player
    This makes movement relative to where the player is facing. 
    */
    private Vector3 CalculateWorldDirection()
    {
        // Converting 2D input into 3D direction (Left/Right = x, forward/backwards = z)
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        // Transforming the local input into the world space based on player rotation
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        return worldDirection.normalized;
    }

    private void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f; // Small downward gravity force to keep player grounded

            if (playerInputHandler.JumpTriggered)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;    
        }
    }

    private void HandleMovement()
    {
        Vector3 worldDirection = CalculateWorldDirection();
        currentMovement.x = worldDirection.x * CurrentSpeed;
        currentMovement.z = worldDirection.z * CurrentSpeed;

        HandleJumping();
        HandleCrouching();

        // Move the player using CharacterController
        characterController.Move(currentMovement * Time.deltaTime);
    }

    // This controls the players left/right movement direction
    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0, rotationAmount, 0);
    }

    // Rotates the camera up/down and is clamped to prevent over-rotation
    private void ApplyVerticalRotation(float rotationAmount)
    {
        verticalRotation = Mathf.Clamp(verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);

        //MICHAEL
        // Apply rotation only to camera (not player body)
        playerCam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    // Handles mouse input and then applies rotation to player and camera
    private void HandleRotation()
    {
        float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity;

        ApplyHorizontalRotation(mouseXRotation);
        ApplyVerticalRotation(mouseYRotation);
    }

    private void HandleCrouching()
    {
        if (playerInputHandler.CrouchTriggered)
        {
            //this.GetComponent(BoxCollider).size -= Vector3(0, crouchHeight, 0);
            //this.GetComponetn(BoxCollider).center -= Vector3(0, crouchHeight, 0);
            Debug.Log("Crouch pressed");
            Debug.Log(characterController.height);
            characterController.height = crouchHeight;
            
        } 
        else
        {
            characterController.height = 2.0f;
        }
    }
}
