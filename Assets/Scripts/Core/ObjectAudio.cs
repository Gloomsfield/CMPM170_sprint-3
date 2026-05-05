using Unity.VisualScripting;
using UnityEngine;

public class ObjectAudio : MonoBehaviour
{
    /* NOTES OF A LUNATIC:
     * I think this might be the best way to handle object audio for a 3D game.
     * Or we can in the audio manager have a function that spawns a temporary audio source
     * at a position in the world and plays a clip, then destroys itself after the clip is done.
     * Im not sure if this would be more efficient than having an audio source on every object, but it might be.
     * For now, I'll go with the simpler approach of having an audio source on each object that needs to play sounds.
     * We can always refactor later if we find that it's causing performance issues. ( I dont think it will)
    */
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dropSFX;
    //[SerializeField] private AudioClip collisionSFX;
    //^^^ We will add fields as we see fit.

    //Maybe we should just make this playSoundSFX(AudioClip clip),
    //and we could maybe also pass in a pitch parameter if we want to add some variation to the sounds for any reason.
    public void PlayDropSFX()
    {
        audioSource.PlayOneShot(dropSFX);
    }
}
