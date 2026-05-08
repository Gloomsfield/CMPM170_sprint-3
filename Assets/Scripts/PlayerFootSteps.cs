using UnityEngine;

public class PlayerFootSteps : MonoBehaviour
{
    FirstPersonController playerController;
    [SerializeField] AudioClip footStepSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<FirstPersonController>();
    }

    void StartWalkingSFX()
    {
        AudioManager.Instance.PlayPlayerSFX(footStepSFX, true);
    }

    //This might cause issues because it just calls to the StopPlayerSFX
    void StopWalkingSFX()
    {
        AudioManager.Instance.StopPlayerSFX();
    }
}
