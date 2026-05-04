using UnityEngine;

public class GrabController : MonoBehaviour, IsGrabbable
{
    private bool grabbed = false;
    private Rigidbody rb;
    private FixedJoint grabJoint;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void ToggleGrab() {
        grabbed = !grabbed;
        if (grabbed) {
            rb.isKinematic = true;
            rb.detectCollisions = false;  // Optional: turns off physics interaction while held
            transform.SetParent(Camera.main.transform);
            /* TO REMOVE Create a joint, which attaches to rigidbodies
            grabJoint = gameObject.AddComponent<FixedJoint>();
            grabJoint.connectedBody = grabber;
            grabJoint.breakForce = Mathf.Infinity;
            */
        } else {
            rb.transform.SetParent(null);
        }
    }
}
