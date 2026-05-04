using UnityEngine;

public class GrabController : MonoBehaviour, IsGrabbable
{
    private bool grabbed = false;
    private Rigidbody rb;
    private FixedJoint grabJoint;

    private Camera grabberHead;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void ToggleGrab(Camera head) {
        grabbed = !grabbed;
        if (grabbed) {
            grabberHead = head;
            rb.isKinematic = true;
            rb.transform.SetParent(grabberHead.transform);
        } else {
            rb.transform.SetParent(null);
            rb.isKinematic = false;
            grabberHead = null;
        }
    }
}
