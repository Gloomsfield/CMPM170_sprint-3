using System.Collections.Generic;

public class EvaluationHandler {
	
	private static EvaluationHandler _instance;

	private EvaluationHandler() { }

	public static EvaluationHandler Instance {
		get {
			_instance ??= new EvaluationHandler();
			return _instance;
		}
	}

	private List<Pattern> _unstartedPatterns = new();
	private List<Pattern> _activePatterns = new();

	public void PropagateBehavior(Behavior behavior) {
		for(int i = _activePatterns.Count - 1; i >= 0; i--) {
			Pattern currentActivePattern = _activePatterns[i];
			if(currentActivePattern.TryCancel(behavior) || currentActivePattern.TryContinue(behavior)) {
				_activePatterns.RemoveAt(i);
			}
		}

		for(int i = _unstartedPatterns.Count - 1; i >= 0; i--) {
			Pattern currentPattern = _unstartedPatterns[i];
			
			if(currentPattern.TryStart(behavior, out Pattern duplicate)) {
				_activePatterns.Add(currentPattern);
				_unstartedPatterns.RemoveAt(i);

				_unstartedPatterns.Add(duplicate);
			}
		}
	}

}

