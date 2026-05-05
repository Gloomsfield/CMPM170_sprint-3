using Unity.VisualScripting;
using UnityEngine;

public class ObjectAudio : MonoBehaviour
{
    [SerializeField] AudioClip dropSFX;

    //plays a noice the frame this object enters/ contacts a collider
    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.PlaySoundOnObject(dropSFX, this.gameObject);
    }
}
