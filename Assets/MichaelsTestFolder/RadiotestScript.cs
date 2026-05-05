using UnityEngine;
using UnityEngine.InputSystem;

public class RadiotestScript : MonoBehaviour
{
    [SerializeField] private AudioClip BGM;
    [SerializeField] private AudioClip BGM2;
    [SerializeField] private AudioClip crashSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayMusic(BGM, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("R key was pressed, changing music.");
            AudioManager.Instance.SetMusicVolume(.3f);
            AudioManager.Instance.PlayMusic(BGM2,true);
        }
    }

}
