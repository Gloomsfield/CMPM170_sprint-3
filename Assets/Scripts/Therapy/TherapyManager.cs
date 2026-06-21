using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class TherapyManager : MonoBehaviour {
    public static TherapyManager Instance { get; private set; }

    [SerializeField] CinemachineCamera playerCam;
    [SerializeField] CinemachineCamera therapyCam;

    private TherapistCamFocus camController;

	private TherapistState _state = new();
	private ResponseGenerator _responseGenerator;
    private BehaviorLog _behaviorLog;

	private string _currentJudgement;

	private List<VerbType> _notableVerbs = new(){
        VerbType.BURNS,
        //VerbType.EATS,
	};

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
			return;
		}

        camController = new TherapistCamFocus(playerCam, therapyCam);
        _behaviorLog = new();

		EventManager.onBehavior += JudgeBehavior;
    }

	void Start() {
		_responseGenerator = new(Resources.Load<TextAsset>("responses").text);
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

		_currentJudgement = _responseGenerator.Generate(null, _state);

		EventManager.InvokeTherapyStarted();
	}

	public void DisplayJudgement() {
		UIManager.Instance.setText(_currentJudgement);

		UIManager.Instance.DisplayTherapyText();

		_state.recentBehavior = null;
		_currentJudgement = "";
	}

    void OnDestroy() {
        camController.Unsuscribe();
    }
}
