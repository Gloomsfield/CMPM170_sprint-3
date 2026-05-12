using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class TherapyManager : MonoBehaviour {
    public static TherapyManager Instance { get; private set; }

    [SerializeField] CinemachineCamera playerCam;
    [SerializeField] CinemachineCamera therapyCam;

    private TherapistCamFocus camController;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);

        camController = new TherapistCamFocus(playerCam, therapyCam);
         
        EventManager.therapyStarted += StartStuckInTherapyTimer;
        EventManager.therapyEnded += StartWaitingForTherapyTimer;

        StartWaitingForTherapyTimer();
    }

    IEnumerator WaitForTherapy() {
        // TODO make dynamic???
        yield return new WaitForSeconds(5);
        EventManager.InvokeTherapyStarted();
    }

    IEnumerator StuckInTherapy() {
        // TODO make dynamic???
        yield return new WaitForSeconds(5);
        EventManager.InvokeTherapyEnded();
    }

    void StartWaitingForTherapyTimer() {
        StartCoroutine(WaitForTherapy());
    }

    void StartStuckInTherapyTimer() {
        StartCoroutine(StuckInTherapy());
    }

    void OnDestroy() {
        camController.Unsuscribe();
        EventManager.therapyStarted -= StartStuckInTherapyTimer;
        EventManager.therapyEnded -= StartWaitingForTherapyTimer;
    }
}
