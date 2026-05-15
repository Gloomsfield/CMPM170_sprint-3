using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class TherapyManager : MonoBehaviour {
    public static TherapyManager Instance { get; private set; }

    [SerializeField] CinemachineCamera playerCam;
    [SerializeField] CinemachineCamera therapyCam;
    [SerializeField] int playTime = 25;
    [SerializeField] int therapyTime = 10;


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
        yield return new WaitForSeconds(playTime); //needs to become an export var for time waiting.
        EventManager.InvokeTherapyStarted();

    }

    IEnumerator StuckInTherapy() {
        // TODO make dynamic???
        yield return new WaitForSeconds(therapyTime);
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
