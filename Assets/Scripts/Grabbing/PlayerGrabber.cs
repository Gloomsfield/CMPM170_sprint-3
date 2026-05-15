using Unity.Cinemachine;
using UnityEngine;

public class PlayerGrabber : MonoBehaviour {

    private LayerMask grabbableMask;
    private ItemGrabbee currentItem;
    private Rigidbody headRb;

    [SerializeField] float grabRange = 3f;
    [SerializeField] CinemachineCamera playerCam;

    //This means isHolding item is true if we have an item and that item is currently grabbed.
    //Its just faster this way then a big if statement if we need to reuse this
    private bool isHoldingItem => currentItem != null && currentItem.isGrabbed;

	private bool _justStartedGrabbing = false;

    void Start() {
        // Select the game layer we can grab things from
        grabbableMask = LayerMask.GetMask("Grabbable");

        // Suscribe TryGrab to the grabToggled event
        EventManager.grabStart += TryGrabStart;
		EventManager.grabEnd += TryGrabEnd;

        // We attach the object to the camera that acts as the player's head
        headRb = playerCam.GetComponent<Rigidbody>();
    }

    /* Sends a raycast from the camera to grabRange on mouse click, calling ToggleGrab()
     * on the hit object. IMPORTANT: grabbable objects be in the grabbable layer */
    void TryGrabStart() {
        if (isHoldingItem) {
            return;
        }

		_justStartedGrabbing = true;

        currentItem = null;

        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);

        Debug.DrawLine(ray.origin,ray.origin + ray.direction * grabRange,Color.red, 2f);

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, grabRange, grabbableMask))
        {
            ItemGrabbee grabbedItem = hit.collider.GetComponent<ItemGrabbee>();

            if (grabbedItem == null)
            {
                return;
            }
            //This is where we call to Grab the item and set it as the current item.
            currentItem = grabbedItem;
            currentItem.Grab(headRb);
        }
    }

	void TryGrabEnd() {
		if(!isHoldingItem) { return; }

		if(_justStartedGrabbing) {
			_justStartedGrabbing = false;
			return;
		}

		currentItem.Drop();
		currentItem = null;
	}

    public CinemachineCamera GetHead() { return playerCam; }

    /* Before an item is destroyed, it must unsuscribe to all
     * events it is subscribed to. Otherwise, the event will try
     * to call functions that no longer exist */
    void OnDestroy() {
        EventManager.grabStart -= TryGrabStart; 
        EventManager.grabEnd -= TryGrabEnd; 
    }
}
