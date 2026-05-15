using System;
using UnityEngine;

/* This class is responsible for defining how an object should behave when it is
 * grabbed.
 */

[RequireComponent(typeof(ItemNounWrapper))]
[RequireComponent(typeof(Rigidbody))]
public class ItemGrabbee : MonoBehaviour {
    [SerializeField] float breakForce = 400f;

    private bool grabbed = false;
    private FixedJoint grabJoint;

    //A public bool so that PlayerGrabber can ref this.
    public bool isGrabbed => grabbed;

	private event Action onGrab;
	private event Action onDrop;
	private event Action onThrow;

	private Vector3 _lastPosition;
	private float _lastDeltaTime;

	void Start() {
		onGrab += GetComponent<ItemNounWrapper>().OnGrab;
		onDrop += GetComponent<ItemNounWrapper>().OnDrop;
		onThrow += GetComponent<ItemNounWrapper>().OnThrow;
	}

	void Update() {
		_lastDeltaTime = Time.deltaTime;
		_lastPosition = transform.position;
	}

    public void Grab(Rigidbody grabberRb)
    {
        grabbed = true;

        grabJoint = gameObject.AddComponent<FixedJoint>();
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
        }

		Vector3 throwVelocity = (transform.position - _lastPosition) / (_lastDeltaTime * 2.5f);

		GetComponent<Rigidbody>().linearVelocity = throwVelocity;

		if(throwVelocity.sqrMagnitude > 10.0f) {
			onThrow.Invoke();
			return;
		}

		onDrop.Invoke();
    }

    private void OnJointBreak()
    {
        grabbed = false;
        grabJoint = null;
    }
}
