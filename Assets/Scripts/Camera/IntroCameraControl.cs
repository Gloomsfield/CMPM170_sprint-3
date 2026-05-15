using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.Playables;

public class IntroCameraControl : MonoBehaviour
{
    [Header("Cinemachine Camera")]
    [SerializeField] CinemachineCamera skyCamera;
    [SerializeField] CinemachineCamera dollyCamera;
    [SerializeField] CinemachineCamera playerCamera;

    [Header("Timing")]
    [SerializeField] float skyViewTime = 2f;
    [SerializeField] float playerCamTime = 2f;

    [Header("Timeline")]
    [SerializeField] PlayableDirector dollyTimeLine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.InvokeIntroSceneStarted();
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        SetCamera(skyCamera);

        yield return new WaitForSeconds(skyViewTime);

        SetCamera(dollyCamera);
        dollyTimeLine.Play();

        yield return new WaitForSeconds((float)dollyTimeLine.duration);

        SetCamera(playerCamera);

        yield return new WaitForSeconds(playerCamTime);
        EventManager.InvokeIntroSceneComplete();
    }

    void SetCamera(CinemachineCamera activeCamera)
    {
        skyCamera.Priority = 10;
        dollyCamera.Priority = 10;
        playerCamera.Priority = 10;

        activeCamera.Priority = 30;
    }
}
