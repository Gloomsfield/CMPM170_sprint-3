using UnityEngine;

public class GrabController : MonoBehaviour, IsGrabbable
{
    private bool grabbed = false;
    private Rigidbody rb;
    private FixedJoint grabJoint;

    private Vector3 lastCamPosition;
    private Vector3 camDelta;
    private Camera grabberHead;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void ToggleGrab(Camera head) {
        grabbed = !grabbed;
        if (grabbed) {
            grabberHead = head;
            lastCamPosition = grabberHead.transform.position;
            rb.isKinematic = true;
            //rb.transform.SetParent(Camera.main.transform);
            /* TO REMOVE Create a joint, which attaches to rigidbodies
            grabJoint = gameObject.AddComponent<FixedJoint>();
            grabJoint.connectedBody = grabber;
            grabJoint.breakForce = Mathf.Infinity;
            */
        } else {
            grabberHead = null;
            //rb.transform.SetParent(null);
        }
    }

    void Update() {
        if (grabbed) {
            grabberHead.transform.position = grabberHead.transform.position + new Vector3 (0f, 0f, 0.01f);
            camDelta = grabberHead.transform.position - lastCamPosition;
            Debug.Log("cam " + camDelta);
            rb.transform.position = rb.transform.position + camDelta;
            Debug.Log("rb " + rb.transform.position);
            lastCamPosition = grabberHead.transform.position;
        }
    }
}
