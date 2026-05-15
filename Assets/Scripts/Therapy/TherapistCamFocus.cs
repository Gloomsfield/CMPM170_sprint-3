using Unity.Cinemachine;
using UnityEngine;

public class TherapistCamFocus
{
    CinemachineCamera playerCam;
    CinemachineCamera therapistCam;

    public TherapistCamFocus(CinemachineCamera playerCam, CinemachineCamera therapistCam) {
        this.playerCam = playerCam;
        this.therapistCam = therapistCam;
        EventManager.therapyStarted += FocusCamOnTherapist;
        EventManager.therapyEnded += FocusCamOnPlayer;
    }

    void FocusCamOnTherapist()
    {
        playerCam.enabled = false;
        therapistCam.enabled = true;
    }

    void FocusCamOnPlayer()
    {
        playerCam.enabled = true;
        therapistCam.enabled = false;
    }

    public void Unsuscribe() {
        EventManager.therapyStarted -= FocusCamOnTherapist;
        EventManager.therapyEnded -= FocusCamOnPlayer;
    }

}
