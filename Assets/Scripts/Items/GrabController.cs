using UnityEngine;

public class GrabController : MonoBehaviour, IsGrabbable
{
    private bool grabbed = false;
    private Rigidbody rb;
    private FixedJoint grabJoint;

    private Vector3 lastCamPosition;
    private Vector3 camDelta;

    void Start() {
        rb = GetComponent<Rigidbody>();
        lastCamPosition = Camera.main.transform.position;
    }

    public void ToggleGrab() {
        grabbed = !grabbed;
        if (grabbed) {
            rb.isKinematic = true;
            //rb.transform.SetParent(Camera.main.transform);
            rb.MovePosition(GetComponent<Camera>().transform.position);
            /* TO REMOVE Create a joint, which attaches to rigidbodies
            grabJoint = gameObject.AddComponent<FixedJoint>();
            grabJoint.connectedBody = grabber;
            grabJoint.breakForce = Mathf.Infinity;
            */
        } else {
            rb.transform.SetParent(null);
        }
    }

    void Update() {
        camDelta = Camera.main.transform.position - lastCamPosition;
        if (grabbed) {
            rb.transform.position = rb.transform.position + camDelta;
        }
        lastCamPosition = Camera.main.transform.position;
    }
}
