using System;
using UnityEngine;
using Unity.Cinemachine;

/* This class is responsible for defining how an object should behave when it is
 * grabbed.
 */

[RequireComponent(typeof(ItemNounWrapper))]
[RequireComponent(typeof(Rigidbody))]
public class ItemGrabbee : MonoBehaviour {
    [SerializeField] float breakForce = 400f;

    private bool grabbed = false;
    private CinemachineCamera holderHead;
    private FixedJoint grabJoint;

    //A public bool so that PlayerGrabber can ref this.
    public bool isGrabbed => grabbed;

	private event Action onGrab;
	private event Action onDrop;

	private Vector3 _lastPosition;
	private float _lastDeltaTime;

	void Start() {
		onGrab += GetComponent<ItemNounWrapper>().OnGrab;
		onDrop += GetComponent<ItemNounWrapper>().OnDrop;
	}

	void Update() {
		_lastDeltaTime = Time.deltaTime;
		_lastPosition = transform.position;
	}

    public void Grab(Rigidbody grabberRb)
    {
        grabbed = true;

        grabJoint = gameObject.AddComponent<FixedJoint>();
        holderHead = grabberRb.gameObject.GetComponent<CinemachineCamera>();
        grabJoint.connectedBody = grabberRb;
        grabJoint.breakForce = breakForce;
        grabJoint.breakTorque = breakForce;

		onGrab.Invoke();
    }

    public void Drop()
    {
        grabbed = false;

        if (grabJoint != null)
        {
            Destroy(grabJoint);
            grabJoint = null;
            holderHead = null;
        }

		GetComponent<Rigidbody>().linearVelocity = (transform.position - _lastPosition) / (_lastDeltaTime * 2.5f);

		onDrop.Invoke();
    }

    private void OnJointBreak()
    {
        grabbed = false;
        grabJoint = null;
    }

    public CinemachineCamera GetHolderHead() { return holderHead; }
}
