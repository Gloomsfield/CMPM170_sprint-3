using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class TherapistCamFocus : MonoBehaviour
{
    [SerializeField] CinemachineCamera playercam;
    [SerializeField] CinemachineCamera therapistCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        therapistCam.enabled = false;
    }

    public void FocusCamOnTherapist()
    {
        playercam.enabled = false;
        therapistCam.enabled = true;
    }

    public void FocusCamOnPlayer()
    {
        playercam.enabled = true;
        therapistCam.enabled = false;
    }
}
