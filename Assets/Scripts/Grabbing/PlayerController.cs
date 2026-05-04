using System;
using Unity.Cinemachine;
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

    [SerializeField] CinemachineCamera playerCam;
    [SerializeField] float grabRange;

    void Start() {
        grabbableMask = LayerMask.GetMask("Grabbable");
    }

    /* Sends a raycast from the camera to grabRange on mouse click, calling ToggleGrab()
     * on the hit object. IMPORTANT: grabbable objects must implement IsGrabbable and
     * be in the grabbable layer */
    void TryGrab() {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        //Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        RaycastHit hit;
        // TO REMOVE Show raycast in scene
        Debug.DrawLine(ray.origin, ray.direction * grabRange * 10, Color.red, 10f, false);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, grabRange, grabbableMask)) {
            grabbing = !grabbing;
            GameObject target = hit.collider.gameObject;
            Debug.Log("Grabbed " + target);
            grabController = target.GetComponent<GrabController>();
            grabController.ToggleGrab(playerCam);
            grabbing = true;
        }
    }

    void Update() {
        // TODO Do we want to use both buttons? change to FixedUpdate?
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Debug.Log("press received");
            if (grabbing) {
                grabController.ToggleGrab(playerCam);
                grabbing = false;
                Debug.Log("Dropped item");
            } else {
                TryGrab();
            }
        }
    }
}
