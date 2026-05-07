using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls; // The input actions asset created in Unity (PlayerInputs)

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player"; // Name of the action map in use (PlayerInputs>Player)

    // These MUST match the action names inside the input action asset EXACTLY
    // This is also case-sensitive. Used to find and bind input actions at runtime
    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string grab = "Grab";
    [SerializeField] private string crouch = "Crouch"; 

    // Interal references to the actual InputAction objects retrieved from the asset
    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction grabAction;
    private InputAction crouchAction;

    // Public read only so FirstPersonController can read them but not change them
    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public bool CrouchTriggered { get; private set; }

    private void Awake()
    {
		EvaluationHandler.Instance.MakePatternsFromJson(Resources.Load<TextAsset>("patterns").text);
        // Find the action map by name from the input action asset
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        // Find each action inside the action map
        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        grabAction = mapReference.FindAction(grab);
        crouchAction = mapReference.FindAction(crouch);

        SubscribeActionValuesToInputEvents();
    }

    /*
    Subscribes the input actions to events so this class
    always stores the current movement, rotation, jump,
    and sprint input values
    */
    private void SubscribeActionValuesToInputEvents()
    {
        // Movement is a Vector2 from WASD
        // W = (0, 1), A = (-1, 0), S = (0, -1), D = (1, 0)
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        // Reset movement when the key is released
        movementAction.canceled += inputInfo => MovementInput =  Vector2.zero;

        // Rotation is a Vector2 from mouse delta
        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        // Reset rotation when there is no mouse movement
        rotationAction.canceled += inputInfo => RotationInput =  Vector2.zero;

        // Jump becomes true when jump is pressed
        jumpAction.performed += inputInfo => JumpTriggered = true;
        // Jump becomes false when jump is released
        jumpAction.canceled += inputInfo => JumpTriggered = false;

        // Sprint becomes true when sprint is held
        sprintAction.performed += inputInfo => SprintTriggered = true;
        // Sprint becomes false when sprint is released
        sprintAction.canceled += inputInfo => SprintTriggered = false;

        // Grab is toggled when left mouse button is pressed
        grabAction.performed += inputInfo => EventManager.InvokeGrabStart();
        grabAction.canceled += inputInfo => EventManager.InvokeGrabEnd();
        // Crouch becomes true when crouch is held
        crouchAction.performed += inputInfo => CrouchTriggered = true;
        // Crouch becomes false when crouch is released
        crouchAction.canceled += inputInfo => CrouchTriggered = false;

    }

    private void OnEnable()
    {
        /* 
        Called automatically by Unity when this component or GameObject is enabled
        WE DO NOT CALL THIS MANUALLY

        This enables the Player action map, which is the collection of input actions
        (movement, rotation, jump, sprint)
        When enabled, Unity starts listening for those inputs and updating our values

        Input can be toggled indirectly by enabling/disbaleing this componet
        or directly by enabling/disableing the action map itself
        */

        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        /*
        Called automatically by Unity when this component or GameObject is disabled
        WE DO NOT CALL THIS MANUALLY

        This disables the Player action map, stopping all input from being read.
        Useful for pausing the game or opening menus
        */
        playerControls.FindActionMap(actionMapName).Disable();
    }

    //TODO: Implement functions to toggle player controls on/off
    //          - For when the player is forced locked at the Therapist
}


/*
HOW TO SETUP PLAYER IN SCENE

1. Create empty GameObject and name it Player
    - Add a CharacterController component
    - Add PlayerInputHandler Script
    - Add FirstPersonController script

MICHAEL
2. Move main camera as a child of Player
    - Reset its local position
    - Move y position up a little for head height
    - Assign main camera reference in FirstPersonController

3. Assign References in FirstPersonController
    - CharacterController -> Player's CharacterController
    - Main camera (step 2)
    - PlayerInputHandler -> Player's PlayerInputHandler

4. Add GameObject capsle called PlayerVisualize
    - Make child of Player
*/
