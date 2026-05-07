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

	private List<Pattern> _unstartedPatterns = new();
	private List<Pattern> _activePatterns = new();

	public void MakePatternsFromJson(string json) {
		List<PatternBlueprint> patternBlueprints = JsonConvert.DeserializeObject<List<PatternBlueprint>>(json);

		foreach(PatternBlueprint blueprint in patternBlueprints) {
			_unstartedPatterns.Add(blueprint.Build());
		}
	}

	public void HandleEvent(NounInstance sub, NounInstance obj, VerbInstance verb) {
		List<Pattern> newPatterns = new();

		for(int i = _unstartedPatterns.Count - 1; i >= 0; i--) {
			if(!_unstartedPatterns[i].TryContinue(sub, obj, verb)) { continue; }

			if(_unstartedPatterns[i].continueConditionCount == 0) {
				EventManager.InvokeBehaviorComplete(sub, obj, _unstartedPatterns[i].verbOnCompletion);

				_unstartedPatterns.Add(_unstartedPatterns[i].blueprint.Build());
				_unstartedPatterns.RemoveAt(i);

				continue;
			}

			newPatterns.Add(_unstartedPatterns[i]);

			_unstartedPatterns.Add(_unstartedPatterns[i].blueprint.Build());
			_unstartedPatterns.RemoveAt(i);
		}

		for(int i = _activePatterns.Count - 1; i >= 0; i--) {
			if(_activePatterns[i].TryCancel(sub, obj, verb)) {
				_activePatterns.RemoveAt(i);

				continue;
			}

			if(!_activePatterns[i].TryContinue(sub, obj, verb)) { continue; }

			if(_activePatterns[i].continueConditionCount != 0) { continue; }

			EventManager.InvokeBehaviorComplete(sub, obj, _activePatterns[i].verbOnCompletion);
			_activePatterns.RemoveAt(i);
		}

		foreach(Pattern pattern in newPatterns) {
			_activePatterns.Add(pattern);
		}
	}

}

