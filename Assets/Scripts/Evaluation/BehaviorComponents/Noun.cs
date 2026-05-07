using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[JsonConverter(typeof(StringEnumConverter))]
public enum NounTag {
	PLAYER,
	DIRT,
	CORPSE,
	COOKABLE,
	FIRE,
	ANY,
}

public class NounInstance {
    
	private readonly HashSet<NounTag> _tags;

	public NounInstance(List<NounTag> tags) {
		_tags = new(tags);
	}

	public bool IsSubsetOf(HashSet<NounTag> tagSuperset) {
		return tagSuperset.Contains(NounTag.ANY) || _tags.IsSubsetOf(tagSuperset);
	}

}

public class NounRestriction {
	
	private readonly HashSet<NounTag> _validTags;

	public NounRestriction(List<NounTag> tags) {
		_validTags = new(tags);
	}

	public bool CheckConformity(NounInstance noun) {
		return noun.IsSubsetOf(_validTags);
	}

}

public class NounRestrictionBlueprint {
	
	[JsonProperty("identifier")]
	public readonly string identifier;

	[JsonProperty("validTags")]
	private readonly List<NounTag> _validTags;

	public NounRestriction Build() {
		return new NounRestriction(_validTags);
	}

}

