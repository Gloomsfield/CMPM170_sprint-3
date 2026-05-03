using UnityEngine;

public class PlayerController : MonoBehaviour {

    private PlayerInputController playerInputController;

    [SerializeField] private float speed;

    enum states {
        IDLE,
        WALKING,
        GRABBING,
        // ADD STATES HERE
    }

    private void Awake()
    {
        playerInputController = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
        Vector3 positionChange = new Vector3(playerInputController.MovementInputVector.x, 0, playerInputController.MovementInputVector.y) * Time.deltaTime * speed;

        transform.position += positionChange;
    }

}
