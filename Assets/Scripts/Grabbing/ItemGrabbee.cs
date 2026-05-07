using UnityEngine;

/* This class is responsible for defining how an object should behave when it is
 * grabbed.
 */

public class ItemGrabbee : MonoBehaviour {
    [SerializeField] float breakForce = 400f;

    private bool grabbed = false;
    private FixedJoint grabJoint;

    //A public bool so that PlayerGrabber can ref this.
    public bool isGrabbed => grabbed;

    public void Grab(Rigidbody grabberRb)
    {
        grabbed = true;

        grabJoint = gameObject.AddComponent<FixedJoint>();
        grabJoint.connectedBody = grabberRb;
        grabJoint.breakForce = breakForce;
        grabJoint.breakTorque = breakForce;
    }

    public void Drop()
    {
        grabbed = false;

        if (grabJoint != null)
        {
            Destroy(grabJoint);
            grabJoint = null;
        }
    }

    private void OnJointBreak()
    {
        grabbed = false;
        grabJoint = null;
    }
}
