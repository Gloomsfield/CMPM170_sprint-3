using System;
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
    [SerializeField] float standHeight = 2.0f;
    [SerializeField] private float crouchSpeed = 10.0f;
    float camOffset = 0.4f;
    bool isCrouching = false;
    bool canMove = false;

    Action enableMovement;
    Action disableMovement;

    [Header("References")]
    [SerializeField] private CharacterController characterController;   // Handling collision and movement
    [SerializeField] private CinemachineCamera playerCam;               // PLayer camera (child of player)
    [SerializeField] private PlayerInputHandler playerInputHandler;     // Handles input system values

    private Vector3 currentMovement;                                    // Stores x, y, z movement velocity
    private float verticalRotation;                                     // Tracks camera up and down rotation
    // Calculates current movement speed with sprinting active or not
    private float CurrentSpeed => walkSpeed * (playerInputHandler.SprintTriggered ? sprintMultiplier : 1);

    [SerializeField] PlayerNoises PN;
    Vector3 lastLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // CURSOR
        // Locks cursor to center of screen and hides it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterController.height = standHeight;
        characterController.center = new Vector3(0, standHeight / 2f, 0);

        playerCam.transform.localPosition = new Vector3(0, (standHeight / 2f) + camOffset, 0);
        PN = GetComponent<PlayerNoises>();

        /* Create lambdas for disabling an enabling movement +
        * suscribe them to therapy events */
        enableMovement = () => canMove = true;
        disableMovement = () => canMove = false;
        EventManager.therapyStarted += disableMovement;
        EventManager.therapyEnded += enableMovement;
        EventManager.IntroSceneComplete += enableMovement;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) {
            PN.StopWalkingSFX();
            return;
        }
        HandleMovement(); 
        HandleRotation();

        if(transform.position != lastLocation && characterController.isGrounded)
        {
            PN.StartWalkingSFX();
        }
        else if (characterController.isGrounded)
        {
            PN.StopWalkingSFX();
        }

        lastLocation = transform.position;
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
                PN.PlayJumpSFX();
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
        if(playerInputHandler.CrouchTriggered && characterController.isGrounded)
        {
            HandleCrouch();
        } 
        else
        {
            HandleStand();
        }

        if (!isCrouching)
        {
            HandleJumping();
        }

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

    private void HandleCrouch()
    {
        isCrouching = true;
        if(characterController.height > crouchHeight) // Check to see if we need to decrease height to reach crouchHeight
        {
            UpdateCharacterHeight(crouchHeight);

            if(characterController.height - 0.05f <= crouchHeight) // Snap at the very end to stop long float calculations
            {
                characterController.height = crouchHeight;
            }
        }
    }

    void HandleStand()
    {
        if(characterController.height < standHeight) // Check to see if we need to increase height to reach standHeight
        {

            float lastHeight = characterController.height; // Used for moving position line 180

            // Checks if an object is above the player to stop them from standing up
            if (Physics.Raycast(transform.position + Vector3.up * characterController.height, Vector3.up, out RaycastHit hit, standHeight))
            {
                // Slightly adjust the height to below the object if crouch is released
                if (hit.distance < standHeight - crouchHeight)
                {
                    UpdateCharacterHeight(crouchHeight + hit.distance);
                    return;
                }
                else
                {
                    UpdateCharacterHeight(standHeight);
                } 
            } 
            else
            {
                UpdateCharacterHeight(standHeight);
            }            

            if(characterController.height + 0.05f >= standHeight) // Snap at the very end to stop long float calculations
            {
                characterController.height = standHeight;
                isCrouching = false;    
            }

            // Changes position so physics body isn't pushed up by ground collision as standing up
            transform.position += new Vector3(0, (characterController.height - lastHeight) / 2, 0);
        }
    }

    // Uses Mathf.Lerp to have a smooth crouching and uncrouching motion. 
    void UpdateCharacterHeight(float newHeight)
    {
        characterController.height = Mathf.Lerp(characterController.height, newHeight, crouchSpeed * Time.deltaTime);

        characterController.center = new Vector3(0, characterController.height / 2f, 0);

        // Move the camera independently for a smoother motion
        playerCam.transform.localPosition = new Vector3(0, (characterController.height / 2) + camOffset, 0);
    }

    void OnDestroy() {
        EventManager.therapyStarted -= enableMovement;
        EventManager.therapyEnded -= disableMovement;
    }
}
