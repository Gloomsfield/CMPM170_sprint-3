using System.Collections.Generic;
using Newtonsoft.Json;

public class EvaluationHandler {
	
	private static EvaluationHandler _instance;

	private EvaluationHandler() { }

	public static EvaluationHandler Instance {
		get {
			_instance ??= new EvaluationHandler();

			EventManager.onBehavior += _instance.HandleEvent;

			return _instance;
		}
	}

	private List<Pattern> _unstartedPatterns;
	private List<Pattern> _activePatterns = new();

	public void MakePatternsFromJson(string json) {
		_unstartedPatterns = JsonConvert.DeserializeObject<List<Pattern>>(json);
	}

	public void HandleEvent(NounInstance sub, NounInstance obj, VerbInstance verb) {
		List<Pattern> newPatterns = new();

		for(int i = _unstartedPatterns.Count - 1; i >= 0; i--) {
			if(!_unstartedPatterns[i].TryContinue(sub, obj, verb)) { continue; }

			newPatterns.Add(_unstartedPatterns[i]);
			_unstartedPatterns.RemoveAt(i);
		}

		for(int i = _activePatterns.Count - 1; i >= 0; i--) {
			if(_activePatterns[i].TryCancel(sub, obj, verb)) {
				_activePatterns.RemoveAt(i);

				continue;
			}

			if(!_activePatterns[i].TryContinue(sub, obj, verb)) { continue; }

			if(_activePatterns[i].continueConditionCount != 0) { continue; }

			EventManager.InvokeBehaviorComplete(_activePatterns[i].verbPast);
		}

		foreach(Pattern pattern in newPatterns) {
			_activePatterns.Add(pattern);
			_unstartedPatterns.Add(pattern.blueprint.Build());
		}
	}

}

