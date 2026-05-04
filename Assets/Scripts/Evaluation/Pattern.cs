using System.Collections.Generic;

public class Condition {
	
	private readonly NounRestriction _subject;
	private readonly NounRestriction _object;
	private readonly VerbRestriction _verb;

	public Condition(NounRestriction condSubject, NounRestriction condObject, VerbRestriction condVerb) {
		_subject = condSubject;
		_object = condObject;
		_verb = condVerb;
	}

	public bool Check(NounInstance checkSubject, NounInstance checkObject, VerbInstance checkVerb) {
		return _subject.CheckConformity(checkSubject) &&
			_object.CheckConformity(checkObject) &&
			_verb.CheckConformity(checkVerb);
	}

}

public class Pattern {

	private readonly List<List<Condition>> _continueConditions;
	private int _conditionIndex;

	private readonly Dictionary<int, List<Condition>> _cancelConditionMap;

	public Pattern(
		List<List<Condition>> continueConditions,
		List<(int, int, Condition)> cancelConditions
	) {
		_continueConditions = continueConditions;

		// we need to be able to get cancellation Conditions quickly,
		// so here we fill out a Dictionary mapping ints to Conditions.
		// the ints represent the indices at which each cancellation
		// Condition is relevant.
		foreach((int start, int end, Condition condition) in cancelConditions) {
			for(int i = start; i < end; i++) {
				_cancelConditionMap.TryAdd(i, new());

				_cancelConditionMap.TryGetValue(i, out List<Condition> conditionList);
				conditionList.Add(condition);
			}
		}
	}

	public bool TryContinue(
		NounInstance trySubject,
		NounInstance tryObject,
		VerbInstance tryVerb,
		out List<Condition> newConditions
	) {
		bool didAdvanceCondition = false;

		foreach(Condition condition in _continueConditions[_conditionIndex]) {
			if(!condition.Check(trySubject, tryObject, tryVerb)) { continue; }

			didAdvanceCondition = true;
			_conditionIndex++;
			break;
		}

		newConditions = _continueConditions[_conditionIndex];
		return didAdvanceCondition;
	}

	public bool TryCancel(
		NounInstance trySubject,
		NounInstance tryObject,
		VerbInstance tryVerb
	) {
		if(!_cancelConditionMap.TryGetValue(
			_conditionIndex, out List<Condition> cancelConditions
		)) { return false; }

		foreach(Condition condition in cancelConditions) {
			if(condition.Check(trySubject, tryObject, tryVerb)) { return true; }
		}

		return false;
	}

}
