using System;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerGrabber : MonoBehaviour {

    private LayerMask grabbableMask;
    private bool grabbing = false;
    private ItemGrabbee itemGrabbee;
    private Rigidbody headRb;

    [SerializeField] float grabRange;
    [SerializeField] CinemachineCamera playerCam;

    void Start() {
        // Select the game layer we can grab things from
        grabbableMask = LayerMask.GetMask("Grabbable");

        // Suscribe TryGrab to the grabToggled event
        EventManager.grabToggled += TryGrab;

        // We attach the object to the camera that acts as the player's head
        headRb = playerCam.GetComponent<Rigidbody>();
    }

    /* Sends a raycast from the camera to grabRange on mouse click, calling ToggleGrab()
     * on the hit object. IMPORTANT: grabbable objects be in the grabbable layer */
    void TryGrab() {
        if (grabbing) {
            itemGrabbee.ToggleGrab(headRb);
            grabbing = false;
        } else {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            RaycastHit hit;
            // TO REMOVE Show raycast in scene view
            Debug.DrawLine(ray.origin, ray.direction * grabRange * 10, Color.red, 10f, false);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, grabRange, grabbableMask)) {
                GameObject target = hit.collider.gameObject;
                itemGrabbee = target.GetComponent<ItemGrabbee>();
                itemGrabbee.ToggleGrab(headRb);
                //Debug.Log("grabbed: " + target);
                grabbing = true;
            }
        }
    }
}
