using Unity.Cinemachine;
using UnityEngine;

/* This class is responsible for defining how an object should behave when it is
 * grabbed.
 */

public class ItemGrabbee : MonoBehaviour {
    [SerializeField] float breakForce = 400f;

    private bool grabbed = false;
    private Rigidbody rb;
    private FixedJoint grabJoint;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    /* Creates a joint between the grabber's body and this item's body to keep them
     * attached to one another */
    public void ToggleGrab(Rigidbody grabberRb) {
        grabbed = !grabbed;
        if (grabbed) {
            Debug.Log("created joint" + grabberRb);
            grabJoint = gameObject.AddComponent<FixedJoint>();
            grabJoint.connectedBody = grabberRb; 
            grabJoint.breakForce = breakForce;
            grabJoint.breakTorque = breakForce;
        } else {
            Destroy(grabJoint);
        }
    }

    void OnJointBreak() {
        //Debug.Log("joint broke");
        grabbed = false;
    }
}
