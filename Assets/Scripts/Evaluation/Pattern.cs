using System.Collections.Generic;
using Newtonsoft.Json;

public class Condition {

	public readonly string subIdentifier;
	public readonly string objIdentifier;

	public readonly VerbRestriction verbRestriction;

	public Condition(
		string subIdentifier,
		string objIdentifier,
		VerbRestriction verbRestriction
	) {
		this.subIdentifier = subIdentifier;
		this.objIdentifier = objIdentifier;
		this.verbRestriction = verbRestriction;
	}

}

public class ConditionBlueprint {
	
	[JsonProperty("subject")]
	private readonly string subIdentifier;
	[JsonProperty("object")]
	private readonly string objIdentifier;

	[JsonProperty("verbRestriction")]
	private readonly VerbRestrictionBlueprint verbRestriction;

	public Condition Build() {
		return new(subIdentifier, objIdentifier, verbRestriction.Build());
	}
}

public class Pattern {

	public readonly VerbType verbOnCompletion;

	public readonly PatternBlueprint blueprint;

	private Dictionary<string, NounRestriction> _nounDeclarations = new();
	private Dictionary<string, NounInstance> _nounDefinitions = new();
	private Dictionary<NounInstance, string> _inverseNounDefinitions = new();

	private List<List<Condition>> _continueConditions = new();
	private List<List<Condition>> _cancelConditions = new();

	public uint continueConditionCount => (uint)_continueConditions.Count;
	public uint cancelConditionCount => (uint)_cancelConditions.Count;

	private uint _conditionCounter = 0;

	public Pattern(VerbType verbOnCompletion, PatternBlueprint blueprint) {
		this.verbOnCompletion = verbOnCompletion;

		this.blueprint = blueprint;
	}

	public Pattern AddNounDeclaration(string identifier, NounRestriction restriction) {
		_nounDeclarations.Add(identifier, restriction);

		return this;
	}

	public Pattern AddContinueConditions(List<Condition> conditions) {
		_continueConditions.Add(conditions);

		return this;
	}

	public Pattern AddCancelConditions(List<Condition> conditions) {
		_cancelConditions.Add(conditions);

		return this;
	}

	private bool TryDefineNoun(string identifier, NounInstance noun) {
		if(!_nounDeclarations.TryGetValue(identifier, out NounRestriction restriction)) { return false; }

		if(_nounDefinitions.ContainsKey(identifier) || _inverseNounDefinitions.ContainsKey(noun)) { return false; }

		_nounDefinitions.Add(identifier, noun);
		_inverseNounDefinitions.Add(noun, identifier);

		return true;
	}

	public bool TryContinue(Behavior behavior) {
		if(_continueConditions.Count < 1) { return false; }

		if(!MeetsCondition(behavior, _continueConditions[0])) { return false; }

		_continueConditions.RemoveAt(0);
		if(_cancelConditions.Count > 0 && _conditionCounter > 0) { _cancelConditions.RemoveAt(0); }
		_conditionCounter++;

		return true;
	}

	public bool TryCancel(Behavior behavior) {
		if(_cancelConditions.Count < 1) { return false; }
		if(_conditionCounter < 1) { return false; }

		if(!MeetsCondition(behavior, _cancelConditions[0])) { return false; }

		return true;
	}

	private bool MeetsCondition(
		Behavior behavior,
		List<Condition> conditions
	) {
		bool subRegistered = _inverseNounDefinitions.TryGetValue(behavior.sub, out string subIdentifier);
		bool objRegistered = _inverseNounDefinitions.TryGetValue(behavior.obj, out string objIdentifier);

		foreach(Condition condition in conditions) {
			if(subRegistered && condition.subIdentifier != subIdentifier) { continue; }
			if(objRegistered && condition.objIdentifier != objIdentifier) { continue; }

			if(!subRegistered && _nounDefinitions.ContainsKey(condition.subIdentifier)) { continue; }
			if(!objRegistered && _nounDefinitions.ContainsKey(condition.objIdentifier)) { continue; }

			if(!_nounDeclarations.TryGetValue(condition.subIdentifier, out NounRestriction subRestriction)) { continue; }
			if(!_nounDeclarations.TryGetValue(condition.objIdentifier, out NounRestriction objRestriction)) { continue; }

			bool subConforms = subRestriction.CheckConformity(behavior.sub);
			bool objConforms = objRestriction.CheckConformity(behavior.obj);
			bool verbConforms = condition.verbRestriction.CheckConformity(behavior.verb);

			if(!subConforms || !objConforms || !verbConforms) { continue; }

			if(!subRegistered) { TryDefineNoun(condition.subIdentifier, behavior.sub); }
			if(!objRegistered) { TryDefineNoun(condition.objIdentifier, behavior.obj); }

			return true;
		}

		return false;
	}
}

public class PatternBlueprint {
	
	[JsonProperty("verbOnCompletion")]
	public VerbType verbOnCompletion;

	[JsonProperty("nounRestrictions")]
	private List<NounRestrictionBlueprint> _nounRestrictionBlueprints;

	[JsonProperty("continueConditions")]
	private List<List<ConditionBlueprint>> _continueConditionBlueprints;
	[JsonProperty("cancelConditions")]
	private List<List<ConditionBlueprint>> _cancelConditionBlueprints;

	public Pattern Build() {
		Pattern res = new(verbOnCompletion, this);

		foreach(NounRestrictionBlueprint restrictionBlueprint in _nounRestrictionBlueprints) {
			res.AddNounDeclaration(restrictionBlueprint.identifier, restrictionBlueprint.Build());
		}

		foreach(List<ConditionBlueprint> conditionBlueprints in _continueConditionBlueprints) {
			List<Condition> conditions = new();

			foreach(ConditionBlueprint conditionBlueprint in conditionBlueprints) {
				conditions.Add(conditionBlueprint.Build());
			}

			res.AddContinueConditions(conditions);
		}

		if(_cancelConditionBlueprints != null) {
			foreach(List<ConditionBlueprint> conditionBlueprints in _cancelConditionBlueprints) {
				List<Condition> conditions = new();
        	
				foreach(ConditionBlueprint conditionBlueprint in conditionBlueprints) {
					conditions.Add(conditionBlueprint.Build());
				}
        	
				res.AddCancelConditions(conditions);
			}
		}

		return res;
	}

}

