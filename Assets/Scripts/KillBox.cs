using UnityEngine;

public class KillBox : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.gameObject.transform.position = spawnPoint.position;
        }
    }
}
