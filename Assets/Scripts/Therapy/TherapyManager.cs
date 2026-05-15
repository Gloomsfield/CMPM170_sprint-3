using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class TherapyManager : MonoBehaviour {
    public static TherapyManager Instance { get; private set; }

    [SerializeField] CinemachineCamera playerCam;
    [SerializeField] CinemachineCamera therapyCam;

    private TherapistCamFocus camController;

	private TherapistState _state;

	private List<VerbType> _notableVerbs = new(){
		VerbType.THROWS,
	};

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);

        camController = new TherapistCamFocus(playerCam, therapyCam);
         
        EventManager.therapyStarted += StartStuckInTherapyTimer;
        EventManager.therapyEnded += StartWaitingForTherapyTimer;

		EventManager.onBehaviorComplete += JudgeBehavior;

        StartWaitingForTherapyTimer();
    }

    IEnumerator WaitForTherapy() {
        // TODO make dynamic???

        yield return new WaitForSeconds(50); //needs to become an export var for time waiting.
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

	void JudgeBehavior(Behavior behavior) {
		bool notable = false;

		foreach(VerbType notableVerb in _notableVerbs) {
			if(!behavior.verb.CompareType(notableVerb)) {
				continue;
			}
			
			notable = true;
			break;
		}

		if(!notable) { return; }

		_state.recentBehavior = behavior;

		ResponseGenerator.Generate(null, _state);
	}

    void OnDestroy() {
        camController.Unsuscribe();
        EventManager.therapyStarted -= StartStuckInTherapyTimer;
        EventManager.therapyEnded -= StartWaitingForTherapyTimer;
    }
}
