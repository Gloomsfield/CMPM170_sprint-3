using UnityEngine;

public class PlayerNoises : MonoBehaviour
{
    FirstPersonController playerController;
    [SerializeField] AudioClip footStepSFX;
    [SerializeField] AudioClip[] jumpSFX;

    bool isPlaying = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<FirstPersonController>();
    }

    public void StartWalkingSFX()
    {
        if (!isPlaying)
        {
            AudioManager.Instance.PlayPlayerSFX(footStepSFX, true);
            isPlaying = true;
        }
    }

    //This might cause issues because it just calls to the StopPlayerSFX
    public void StopWalkingSFX()
    {
        AudioManager.Instance.StopPlayerSFX();
        isPlaying = false;
    }

    public void PlayJumpSFX()
    {
        AudioManager.Instance.PlayPlayerSFX(jumpSFX[Random.Range(0,3)],false);
        isPlaying = false;
    }
}
