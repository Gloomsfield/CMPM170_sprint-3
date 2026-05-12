using Unity.VisualScripting;
using UnityEngine;

public class ItemCollisionManager : MonoBehaviour
{
    [SerializeField] AudioClip dropSFX;

    //plays a noice the frame this object enters/ contacts a collider
    private void OnCollisionEnter(Collision collision)
    {
        EventManager.InvokeItemCollided(dropSFX, this.gameObject, null);
    }
}
