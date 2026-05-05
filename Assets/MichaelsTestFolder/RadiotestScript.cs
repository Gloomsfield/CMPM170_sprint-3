using UnityEngine;

public class RadiotestScript : MonoBehaviour
{
    [SerializeField] private AudioClip BGM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.SetMusicVolume(.01f);
        AudioManager.Instance.PlayMusic(BGM, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
