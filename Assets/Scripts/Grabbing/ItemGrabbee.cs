using System;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

/* This class is responsible for defining how an object should behave when it is
 * grabbed.
 */

[RequireComponent(typeof(ItemNounWrapper))]
[RequireComponent(typeof(Rigidbody))]
public class ItemGrabbee : MonoBehaviour {
    [SerializeField] float breakForce = 400f;
    private const int throwingVelocityFrameOffest = 3;

    private bool grabbed = false;
    private CinemachineCamera holderHead;
    private FixedJoint grabJoint;

    //A public bool so that PlayerGrabber can ref this.
    public bool isGrabbed => grabbed;

	private event Action onGrab;
	private event Action onDrop;
	private event Action onThrow;

	private Vector3 _lastPosition;
	private float _lastDeltaTime;


    // Keep a queue of last positions, prepend currents frames position, delete
    // anything past ten frames, always pull from the last item in the list to
    // create a sort of "wolf timer" for throwing
     
    private Queue<Vector3> _lastPositions;

	void Start() {
        _lastPositions = new();

		onGrab += GetComponent<ItemNounWrapper>().OnGrab;
		onDrop += GetComponent<ItemNounWrapper>().OnDrop;
		onThrow += GetComponent<ItemNounWrapper>().OnThrow;
	}

	void Update() {
		_lastDeltaTime = Time.deltaTime;
        _lastPositions.Enqueue(transform.position);
        if (_lastPositions.Count > throwingVelocityFrameOffest) {
            _lastPositions.Dequeue();
        }
        _lastPosition = _lastPositions.Peek();
		//_lastPosition = transform.position;
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

		Vector3 throwVelocity = (transform.position - _lastPosition) / (_lastDeltaTime);

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

    public CinemachineCamera GetHolderHead() { return holderHead; }
}
