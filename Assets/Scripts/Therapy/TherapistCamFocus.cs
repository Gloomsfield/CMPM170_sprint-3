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
		CinemachineCore.BlendFinishedEvent.AddListener(DisplayJudgementWrapper);

        playerCam.enabled = false;
        therapistCam.enabled = true;
    }

	private void DisplayJudgementWrapper(ICinemachineMixer _1, ICinemachineCamera _2) {
		TherapyManager.Instance.DisplayJudgement();
		
		CinemachineCore.BlendFinishedEvent.RemoveListener(DisplayJudgementWrapper);
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
