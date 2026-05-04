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
    private bool grabbing = false;
    private GrabController grabController;

    [SerializeField] float grabRange;

    void Start() {
        grabbableMask = LayerMask.GetMask("Grabbable");
    }

    void Update() {
        /* Sends a raycast from the camera to grabRange on mouse click, calling ToggleGrab()
         * on the hit object. IMPORTANT: grabbable objects must implement IsGrabbable and
         * be in the grabbable layer */
        // TODO Do we want to use both buttons? change to FixedUpdate?
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            if (!grabbing) {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit hit;
                // TO REMOVE Show raycast in scene
                Debug.DrawLine(ray.origin, ray.direction * grabRange, Color.red, 10f, false);
                if (Physics.Raycast(ray.origin, ray.direction, out hit, grabRange, grabbableMask)) {
                    grabbing = !grabbing;
                    GameObject target = hit.collider.gameObject;
                    grabController = target.GetComponent<GrabController>();
                    grabController.ToggleGrab();
                }
            }
            grabController.ToggleGrab();
        }
    }
}
