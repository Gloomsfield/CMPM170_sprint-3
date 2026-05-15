using Newtonsoft.Json;
using System.Collections.Generic;

public class ResponseFormat {

	[JsonProperty("format")]
	private readonly string _format;

	public string Format(Behavior behavior) {
		return _format.Replace($"{{subject}}", behavior.sub.name)
			.Replace($"{{object}}", behavior.obj.name.ToUpper())
			.Replace($"{{verb.past.simple}}", behavior.verb.Conjugate(VerbTenseType.SIMPLE_PAST).ToUpper())
			.Replace($"{{verb.present.simple}}", behavior.verb.Conjugate(VerbTenseType.SIMPLE_PRESENT).ToUpper())
			.Replace($"{{verb.future.simple}}", behavior.verb.Conjugate(VerbTenseType.SIMPLE_FUTURE).ToUpper())
			.Replace($"{{verb.past.continuous}}", behavior.verb.Conjugate(VerbTenseType.CONTINUOUS_PAST).ToUpper())
			.Replace($"{{verb.present.continuous}}", behavior.verb.Conjugate(VerbTenseType.CONTINUOUS_PRESENT).ToUpper())
			.Replace($"{{verb.future.continuous}}", behavior.verb.Conjugate(VerbTenseType.CONTINUOUS_FUTURE).ToUpper())
			.Replace($"{{verb.past.perfect}}", behavior.verb.Conjugate(VerbTenseType.PERFECT_PAST).ToUpper())
			.Replace($"{{verb.present.perfect}}", behavior.verb.Conjugate(VerbTenseType.PERFECT_PRESENT).ToUpper())
			.Replace($"{{verb.future.perfect}}", behavior.verb.Conjugate(VerbTenseType.PERFECT_FUTURE).ToUpper());
	}

}

public class ResponseGenerator {

	private List<ResponseFormat> _formats;

	public ResponseGenerator(string formatJson) {
		_formats = JsonConvert.DeserializeObject<List<ResponseFormat>>(formatJson);
	}

	public string Generate(BehaviorLog log, TherapistState state) {
		int index = new System.Random().Next(_formats.Count);

		return _formats[index].Format(state.recentBehavior);
	}

}

