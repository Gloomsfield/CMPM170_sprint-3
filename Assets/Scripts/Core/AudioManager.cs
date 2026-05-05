using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /*
     * We need to audio sources because we want to be able to play music and UI sounds at the same time.
     * The music source will be used for background music, while the UI source will be used for sound effects like button clicks, etc.
     */

    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioSource uiSource;
    [SerializeField] private float musicVolume;
    [SerializeField] private float sfxVolume;


    private void Awake()
    {
        // This is the classic singleton pattern for Unity. It ensures that only one instance of AudioManager exists and persists across scenes.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Parameters: clip - the audio clip to play, loop - whether the music should loop (default is true)
    // This method checks if the clip is null, and if it's already playing to avoid restarting it unnecessarily. If not, it sets the clip, loop setting, and plays the music.
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    //Parameters: clip - the audio clip to play as a one shot effect.
    // This doesnt take loop parameter because we would never want a UI sound to behave that way.
    public void PlayUISound(AudioClip clip)
    {
        if (clip == null) return;
        uiSource.PlayOneShot(clip);
    }

    //Just stops the music. (10x moment)
    public void StopMusic()
    {
        musicSource.Stop();
    }

    //just a simple setter for the music volume.
    public void SetMusicVolume(float volume)
    {
        // we honestly might not need this musicVolume var, but I thought it might come in handy later?
        musicVolume = volume;
        musicSource.volume=volume;
    }


}

