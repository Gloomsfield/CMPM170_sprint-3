using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerCamController : MonoBehaviour {
    [SerializeField] float xSens = 1f;
    [SerializeField] float ySens = 1f;

    private float xRotation = 0;
    private float yRotation = 0;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /* Moves the camera with the mouse, allowing the player to look around */
    void Update() {
        Vector2 rotationDelta = Mouse.current.delta.ReadValue();
        xRotation -= rotationDelta.y;
        yRotation += rotationDelta.x;
        // Clamp so that we cannot look behind ourselves
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
