using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    enum states {
        IDLE,
        WALKING,
        GRABBING,
        // ADD STATES HERE
    }

    private LayerMask grabbableMask;

    [SerializeField] float grabRange;
    
    void Start() {
        grabbableMask = LayerMask.GetMask("Grabbable");
    }

    void Update() {
        // TODO Do we want to use both buttons? change to FixedUpdate?
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            // TO REMOVE Show raycast in scene
            Debug.DrawLine(ray.origin, ray.direction * grabRange, Color.red, 10f, false);
            Debug.Log(Physics.Raycast(ray.origin, ray.direction, grabRange, grabbableMask));
        }
    }


}
