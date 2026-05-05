using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum VerbType {
	GRABS,
	DROPS,
	THROWS,
	ENTERS_VOLUME,
	EXITS_VOLUME,
}

public class VerbInstance {
    
	private readonly VerbType _type;
	private readonly Dictionary<string, float> _parameters;

	VerbInstance(VerbType type, (string, float)[] parameters) {
		_type = type;

		foreach((string name, float value) in parameters) {
			try {
				_parameters.Add(name, value);
			} catch (ArgumentException) {
				// TODO - logging system
			}
		}
	}

	public bool CompareType(VerbType type) { return _type == type; }

	public bool CheckParameterValidity(string name, float min, float max) {
		if(!_parameters.TryGetValue(name, out float value)) { return false;	}

		return (min < value) && (value < max);
	}

}

public class VerbRestriction {
	
	private readonly VerbType _type;
	private readonly List<(string, (float, float))> _parameterRanges;

	public VerbRestriction(VerbType type, List<(string, (float, float))> parameterRanges) {
		_type = type;
		_parameterRanges = parameterRanges;
	}

	public bool CheckConformity(VerbInstance verb) {
		if(!verb.CompareType(_type)) { return false; }

		foreach((string name, (float parameter1, float parameter2)) in _parameterRanges) {
			if(!verb.CheckParameterValidity(name, parameter1, parameter2)) { return false; }
		}

		return true;
	}

}

public class VerbParameterRestrictionBlueprint {
	
	[JsonProperty("name")]
	public readonly string name;

	[JsonProperty("min")]
	public readonly float min;

	[JsonProperty("max")]
	public readonly float max;

}

public class VerbRestrictionBlueprint {
	
	[JsonProperty("verb")]
	private readonly VerbType _type;

	[JsonProperty("parameters")]
	private readonly List<VerbParameterRestrictionBlueprint> _parameters;

	public VerbRestriction Build() {
		List<(string, (float, float))> ranges = new();
		foreach(VerbParameterRestrictionBlueprint parameter in _parameters) {
			ranges.Add((parameter.name, (parameter.min, parameter.max)));
		}

		return new VerbRestriction(_type, ranges);
	}

}

