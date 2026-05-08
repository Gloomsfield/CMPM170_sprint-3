using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class TherapistCamFocus : MonoBehaviour
{
    [SerializeField] CinemachineCamera playercam;
    [SerializeField] CinemachineCamera therapistCam;

    void Start()
    {
        therapistCam.enabled = false;
        EventManager.therapyStarted += FocusCamOnTherapist;
        EventManager.therapyEnded += FocusCamOnPlayer;
        StartCoroutine(waitForTherapy());
    }

    IEnumerator waitForTherapy() {
        // TODO make dynamic???
        yield return new WaitForSeconds(5);
        EventManager.invokeTherapyStarted();
    }

    IEnumerator stuckInTherapy() {
        // TODO make dynamic???
        yield return new WaitForSeconds(5);
        EventManager.invokeTherapyEnded();
    }

    void FocusCamOnTherapist()
    {
        playercam.enabled = false;
        therapistCam.enabled = true;
        StartCoroutine(stuckInTherapy());
    }


    public void FocusCamOnPlayer()
    {
        playercam.enabled = true;
        therapistCam.enabled = false;
        StartCoroutine(waitForTherapy());
    }

    void OnDestroy() {
        EventManager.therapyStarted -= FocusCamOnTherapist;
    }
}
