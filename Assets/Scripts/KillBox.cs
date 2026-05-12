using UnityEngine;

public class KillBox : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    

    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        CharacterController controller = other.GetComponent<CharacterController>();

        if (controller == null)
        {
            return;
        }
        controller.enabled = false;
        other.transform.position = spawnPoint.position;
        controller.enabled = true;
    }
}
