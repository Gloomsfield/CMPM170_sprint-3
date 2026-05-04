using Unity.Cinemachine;
using UnityEngine;

public class GrabController : MonoBehaviour, IsGrabbable
{
    private bool grabbed = false;
    private Rigidbody rb;
    private FixedJoint grabJoint;

    private CinemachineCamera grabberHead;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void ToggleGrab(CinemachineCamera head) {
        grabbed = !grabbed;
        if (grabbed) {
            grabJoint = gameObject.AddComponent<FixedJoint>();
            grabJoint.connectedBody = head.GetComponent<Rigidbody>();
            grabJoint.breakForce = 100f;
            grabJoint.breakTorque = 100f;
        } else {
            Destroy(grabJoint);
            grabberHead = null;
        }
    }

    void OnJointBreak() {
        grabbed = false;
    }

}
