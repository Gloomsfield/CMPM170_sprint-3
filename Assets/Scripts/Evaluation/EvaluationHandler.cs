using System.Collections.Generic;
using Newtonsoft.Json;


/// <summary>
/// The singleton class <c>EvaluationHandler</c> handles classifying game-world events
/// by validating behaviors with ongoing <see cref="Pattern">Patterns</see>.
/// </summary>
public class EvaluationHandler {
	
	private static EvaluationHandler _instance;

	private EvaluationHandler() { }

	public static EvaluationHandler Instance {
		get {
			_instance ??= new EvaluationHandler();

			EventManager.onBehavior += _instance.HandleBehavior;

			return _instance;
		}
	}

	// _unstartedPatterns contains Patterns that have been declared in
	// patterns.json but have not progressed beyond their initial state.
	private List<Pattern> _unstartedPatterns = new();

	// _activePatterns contains Patterns that have progressed beyond
	// their initial state at least once. patterns are removed from
	// this list once they are completed. any Pattern moved to this
	// list from _unstartedPatterns should instantiate a new version
	// of itself via its blueprint to be added once again to
	// _unstartedPatterns.
	private List<Pattern> _activePatterns = new();

	/// <summary>
	/// The member function MakePatternsFromJson populates a list of
	/// Patterns based on the <paramref name="json">passed json
	/// string<paramref/>.
	/// </summary>
	/// <param name="json">the json string from which to build the
	/// <see cref="Pattern">Patterns</see> that will be recognized
	/// by this system.</param>
	public void MakePatternsFromJson(string json) {
		List<PatternBlueprint> patternBlueprints = JsonConvert.DeserializeObject<List<PatternBlueprint>>(json);

		foreach(PatternBlueprint blueprint in patternBlueprints) {
			_unstartedPatterns.Add(blueprint.Build());
		}
	}

	/// <summary>
	/// The member function HandleEvent determines how to process
	/// an incoming event using that event's <param name="behavior"/>.
	/// </summary>
	/// <param name="behavior">The behavior to handle.</param>
	public void HandleBehavior(Behavior behavior) {
		List<Pattern> newPatterns = new();

		// TODO flip order of _unstartedPatterns checks and _activePatterns checks
		// to prevent needing the newPatterns list

		for(int i = _unstartedPatterns.Count - 1; i >= 0; i--) {
			if(!_unstartedPatterns[i].TryContinue(behavior)) { continue; }

			if(_unstartedPatterns[i].continueConditionCount == 0) {
				EventManager.InvokeBehaviorComplete(new(
					behavior.sub, behavior.obj,
					new(_unstartedPatterns[i].verbOnCompletion, new())
				));

				_unstartedPatterns.Add(_unstartedPatterns[i].blueprint.Build());
				_unstartedPatterns.RemoveAt(i);

				continue;
			}

			newPatterns.Add(_unstartedPatterns[i]);

			_unstartedPatterns.Add(_unstartedPatterns[i].blueprint.Build());
			_unstartedPatterns.RemoveAt(i);
		}

		for(int i = _activePatterns.Count - 1; i >= 0; i--) {
			if(_activePatterns[i].TryCancel(behavior)) {
				_activePatterns.RemoveAt(i);

				continue;
			}

			if(!_activePatterns[i].TryContinue(behavior)) { continue; }

			if(_activePatterns[i].continueConditionCount != 0) { continue; }

			EventManager.InvokeBehaviorComplete(new(behavior.sub, behavior.obj, new(_activePatterns[i].verbOnCompletion, new())));
			_activePatterns.RemoveAt(i);
		}

		foreach(Pattern pattern in newPatterns) {
			_activePatterns.Add(pattern);
		}
	}

}

