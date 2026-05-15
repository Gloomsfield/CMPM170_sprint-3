using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class TherapyManager : MonoBehaviour {
    public static TherapyManager Instance { get; private set; }

    [SerializeField] CinemachineCamera playerCam;
    [SerializeField] CinemachineCamera therapyCam;
    [SerializeField] int playTime = 25;
    [SerializeField] int therapyTime = 10;


    private TherapistCamFocus camController;

	private TherapistState _state = new();
	private ResponseGenerator _responseGenerator;

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

		EventManager.onBehavior += JudgeBehavior;

        StartWaitingForTherapyTimer();
    }

	void Start() {
		_responseGenerator = new(Resources.Load<TextAsset>("responses").text);
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

	void JudgeBehavior(Behavior behavior) {
		if(_state.recentBehavior != null) { return; }
		
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

		

		Debug.Log(_responseGenerator.Generate(null, _state));
	}

    void OnDestroy() {
        camController.Unsuscribe();
        EventManager.therapyStarted -= StartStuckInTherapyTimer;
        EventManager.therapyEnded -= StartWaitingForTherapyTimer;
    }
}
