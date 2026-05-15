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
    [SerializeField] private AudioSource playerSource;
    [SerializeField] private float musicVolume;
    [SerializeField] private float uiVolume;
    [SerializeField] private float sfxVolume;
    [SerializeField] private AudioClip therapistBleep;


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

    void Start() {
        EventManager.itemCollided += PlaySoundOnObject; 
    }

    //Parameters: clip - the audio clip to play, loop - whether the music should loop (default is true)
    // This method checks if the clip is null, and if it's already playing to avoid restarting it unnecessarily. If not, it sets the clip, loop setting, and plays the music.
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null)
        {
            return;
        }
        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    //Parameters: clip - the audio clip to play as a one shot effect.
    // This doesnt take loop parameter because we would never want a UI sound to behave that way.
    public void PlayUISound(AudioClip clip)
    {
        if (clip == null)
        {
         return;
        }
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
    public void SetUIVolume(float volume)
    {
        // we honestly might not need this musicVolume var, but I thought it might come in handy later?
        uiVolume = volume;
        uiSource.volume = volume;
    }

    private AudioSourceConfig NewSourceConfig() {
        return new AudioSourceConfig(1.0f);
    }

    //Parameters: Sound - the soundclip that will play, item - the gameobject the source will get added to.
    public void PlaySoundOnObject(AudioClip sound, GameObject item,AudioSourceConfig config)
    {
        config ??= NewSourceConfig();
        
        if (sound == null)
        {
            return;
        }
        //Might be a better way to pack all this up, as there are even more audioSource variables we might want to tune.
        AudioSource itemSource = item.AddComponent<AudioSource>();
        itemSource.clip = sound;
        itemSource.spatialBlend = config.spatialBlend;
        itemSource.minDistance = config.minDistance;
        itemSource.maxDistance = config.maxDistance;
        itemSource.pitch = config.pitch;
        itemSource.volume = config.volume;
        itemSource.Play();
        Destroy(itemSource, sound.length);
    }

    /* Before an item is destroyed, it must unsuscribe to all
     * events it is subscribed to. Otherwise, the event will try
     * to call functions that no longer exist */
    void OnDestroy() {
        EventManager.itemCollided -= PlaySoundOnObject; 
    }

    public float getSFXVolume()
    {
        return sfxVolume;
    }

    public void PlayPlayerSFX(AudioClip clip, bool isLooping = false) {
        if (clip == null)
        {
            return;
        }
        playerSource.clip = clip;
        playerSource.loop = isLooping;
        playerSource.volume = sfxVolume;
        playerSource.Play();
    }
    public void StopPlayerSFX()
    {
        playerSource.Stop();
    }
    public void PlayTherapistLetter(float pitch)
    {
        uiSource.pitch = pitch;
        if (!uiSource.isPlaying)
        {
            uiSource.PlayOneShot(therapistBleep);
        }
    }
}

