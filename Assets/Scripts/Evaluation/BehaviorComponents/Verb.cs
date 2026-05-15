using System;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

/// <summary>
/// The <c>VerbTense</c> enum defines the potential tenses
/// for conjugating a <see cref="VerbType"/>.
/// </summary>
public enum VerbTenseType {
	SIMPLE_PAST,
	SIMPLE_PRESENT,
	SIMPLE_FUTURE,
	CONTINUOUS_PAST,
	CONTINUOUS_PRESENT,
	CONTINUOUS_FUTURE,
	PERFECT_PAST,
	PERFECT_PRESENT,
	PERFECT_FUTURE,
}

public class VerbTense {

	public string simple;
	public string continuous;
	public string perfect;

	public VerbTense(string simple, string continuous, string perfect) {
		this.simple = simple;
		this.continuous = continuous;
		this.perfect = perfect;
	}

}

[AttributeUsage(AttributeTargets.Field)]
public class VerbTensesAttribute : Attribute {
	
	public VerbTense past;
	public VerbTense present;
	public VerbTense future;

	public VerbTensesAttribute(
		string simplePast, string continuousPast, string perfectPast,
		string simplePresent, string continuousPresent, string perfectPresent,
		string simpleFuture, string continuousFuture, string perfectFuture
	) {
		past = new(simplePast, continuousPast, perfectPast);
		present = new(simplePresent, continuousPresent, perfectPresent);
		future = new(simpleFuture, continuousFuture, perfectFuture);
	}

	/// <summary>
	/// Conjugates a <see cref="VerbType"/>.
	/// </summary>
	/// <returns>
	/// A string representing the conjugated form of this
	/// <see cref="VerbType"/>.
	/// </returns>
	public string Conjugate(VerbTenseType tense) {
		if(tense == VerbTenseType.SIMPLE_PAST) { return past.simple; }
		if(tense == VerbTenseType.SIMPLE_PRESENT) { return present.simple; }
		if(tense == VerbTenseType.SIMPLE_FUTURE) { return future.simple; }
		if(tense == VerbTenseType.CONTINUOUS_PAST) { return past.continuous; }
		if(tense == VerbTenseType.CONTINUOUS_PRESENT) { return present.continuous; }
		if(tense == VerbTenseType.CONTINUOUS_FUTURE) { return future.continuous; }
		if(tense == VerbTenseType.PERFECT_PAST) { return past.perfect; }
		if(tense == VerbTenseType.PERFECT_PRESENT) { return present.perfect; }
		if(tense == VerbTenseType.PERFECT_FUTURE) { return future.perfect; }

		return "<VERB COULD NOT BE CONJUGATED>";
	}

}

/// <summary>
/// Possible verbs should be defined here as <c>VerbTypes</c>.
/// This should be done according to the following structure:
/// <code>
/// [VerbTenses("{past tense 2nd person}", "{present tense 2nd person}", "{future tense 2nd person}")]
/// {PRESENT TENSE 3rd PERSON},
/// </code>
/// where anything between curly braces should be replaced by
/// the indicated verb.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum VerbType {

	// atomic verbs
	[VerbTenses(
		"grabbed", "grabbing", "grabbed",
		"grab", "grabbing", "grabbed",
		"grab", "grabbing", "grabbed"
	)]
	GRABS,

	[VerbTenses(
		"dropped", "dropping", "dropped",
		"drop", "dropping", "dropped",
		"drop", "dropping", "dropped"
	)]
	DROPS,

	[VerbTenses(
		"threw", "throwing", "thrown",
		"throw", "throwing", "thrown",
		"throw", "throwing", "thrown"
	)]
	THROWS,

	[VerbTenses(
		"entered", "entering", "entered",
		"enter", "entering", "entered",
		"enter", "entering", "entered"
	)]
	ENTERS_VOLUME,

	[VerbTenses(
		"exited", "exiting", "exited",
		"exit", "exiting", "exited",
		"exit", "exiting", "exited"
	)]
	EXITS_VOLUME,

	// compound verbs
	[VerbTenses(
		"ate", "eating", "eaten",
		"eat", "eating", "eaten",
		"eat", "eating", "eaten"
	)]
	EATS,

}

public static class VerbTypeExtensions {

	/// <summary>
	/// The <see cref="VerbType"/> extension conjugation method.
	/// </summary>
	/// <returns>
	/// A string representing the conjugated form of this
	/// <see cref="VerbType"/>.
	/// </returns>
	public static string Conjugate(this VerbType verbType, VerbTenseType tense) {
		VerbTensesAttribute attribute = typeof(VerbType).GetTypeInfo()
			.GetField(verbType.ToString())
			.GetCustomAttribute(typeof(VerbTensesAttribute))
			as VerbTensesAttribute;
		return attribute.Conjugate(tense);
	}

}

/// <summary>
/// The <c>VerbInstance</c> class represents the verb in
/// an action that has just occurred.
/// </summary>
public class VerbInstance {
    
	private readonly VerbType _type;
	private readonly Dictionary<string, float> _parameters;

	/// <param name="type">The <see cref="VerbType"/> that describes this
	/// instance.</param>
	/// <param name="parameters">Any parameters associated with this
	/// instance.</param>
	public VerbInstance(VerbType type, List<(string, float)> parameters) {
		_type = type;

		foreach((string name, float value) in parameters) {
			try {
				_parameters.Add(name, value);
			} catch (ArgumentException) {
				// TODO - logging system
			}
		}
	}

	/// <param name="type"> The <see cref="VerbType"/> with which
	/// to compare this <c>VerbInstance</c>.
	public bool CompareType(VerbType type) { return _type == type; }

	/// <param name="name">The name of the parameter to be checked.</param>
	/// <param name="min">The minimum accepted value for this parameter.</param>
	/// <param name="max">The maximum accepted value for this parameter.</param>
	public bool CheckParameterValidity(string name, float min, float max) {
		if(!_parameters.TryGetValue(name, out float value)) { return false;	}

		return (min < value) && (value < max);
	}

	public string Conjugate(VerbTenseType tense) {
		return _type.Conjugate(tense);
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

		if(_parameters == null) { return new VerbRestriction(_type, ranges); }

		foreach(VerbParameterRestrictionBlueprint parameter in _parameters) {
			ranges.Add((parameter.name, (parameter.min, parameter.max)));
		}

		return new VerbRestriction(_type, ranges);
	}

}

