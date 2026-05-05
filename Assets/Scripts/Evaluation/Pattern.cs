using System.Collections.Generic;

public class Condition {
	
	private readonly NounRestriction _sub;
	private readonly NounRestriction _obj;
	private readonly VerbRestriction _verb;

	public Condition(NounRestriction condSubject, NounRestriction condObject, VerbRestriction condVerb) {
		_sub = condSubject;
		_obj = condObject;
		_verb = condVerb;
	}

	public bool Check(Behavior behavior) {
		return _sub.CheckConformity(behavior.sub) &&
			_obj.CheckConformity(behavior.obj) &&
			_verb.CheckConformity(behavior.verb);
	}

}

public class Behavior {
	
	public NounInstance sub;
	public NounInstance obj;
	public VerbInstance verb;

	public Behavior(NounInstance sub, NounInstance obj, VerbInstance verb) {
		this.sub = sub;
		this.obj = obj;
		this.verb = verb;
	}

}

public class Pattern {

	private readonly List<List<Condition>> _continueConditions;
	private int _conditionIndex;

	private readonly Dictionary<int, List<Condition>> _cancelConditionMap;

	public Pattern(
		List<List<Condition>> continueConditions,
		List<(int, int, Condition)> cancelConditionBounds
	) {
		_continueConditions = continueConditions;

		// we need to be able to get cancellation Conditions quickly,
		// so here we fill out a Dictionary mapping ints to Conditions.
		// the ints represent the indices at which each cancellation
		// Condition is relevant.
		foreach((int start, int end, Condition condition) in cancelConditionBounds) {
			for(int i = start; i < end; i++) {
				_cancelConditionMap.TryAdd(i, new());

				_cancelConditionMap.TryGetValue(i, out List<Condition> conditionList);
				conditionList.Add(condition);
			}
		}
	}

	public Pattern(
		List<List<Condition>> continueConditions,
		Dictionary<int, List<Condition>> cancelConditionMap
	) {
		_continueConditions = continueConditions;
		_cancelConditionMap = cancelConditionMap;
	}

	public bool TryStart(Behavior behavior, out Pattern clonedPattern) {
		clonedPattern = null;

		bool didStart = false;

		foreach(Condition condition in _continueConditions[0]) {
			if(!condition.Check(behavior)) { continue; }

			clonedPattern = new Pattern(
				_continueConditions,
				_cancelConditionMap
			);

			_conditionIndex++;
			didStart = true;
			break;
		}

		return didStart;
	}

	public bool TryContinue(Behavior behavior) {
		foreach(Condition condition in _continueConditions[0]) {
			if(!condition.Check(behavior)) { continue; }

			_conditionIndex++;
			_continueConditions.RemoveAt(0);
			break;
		}

		return _continueConditions.Count == 0;
	}

	public bool TryCancel(Behavior behavior) {
		if(!_cancelConditionMap.TryGetValue(
			_conditionIndex, out List<Condition> cancelConditions
		)) { return false; }

		foreach(Condition condition in cancelConditions) {
			if(condition.Check(behavior)) { return true; }
		}

		return false;
	}

}

