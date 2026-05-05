using Unity.VisualScripting;
using UnityEngine;

public class ObjectAudio : MonoBehaviour
{
    [SerializeField] AudioClip dropSFX;

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.PlaySoundOnObject(dropSFX, this.gameObject);
    }
}
