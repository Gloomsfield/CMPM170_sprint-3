using Newtonsoft.Json;
using System.Collections.Generic;

public class ResponseFormat {

	[JsonProperty("format")]
	private readonly string _format;

	public string Format(Behavior behavior) {
		return _format.Replace($"{{subject}}", behavior.sub.name)
			.Replace($"{{object}}", behavior.obj.name)
			.Replace($"{{verb.past}}", behavior.verb.Conjugate(VerbTense.PAST))
			.Replace($"{{verb.present}}", behavior.verb.Conjugate(VerbTense.PRESENT))
			.Replace($"{{verb.future}}", behavior.verb.Conjugate(VerbTense.FUTURE));
	}

}

public class ResponseGenerator {

	private static List<ResponseFormat> _formats;

	public static string Generate(BehaviorLog log, TherapistState state) {
		int index = new System.Random().Next(_formats.Count);

		return _formats[index].Format(state.recentBehavior);
	}

}

