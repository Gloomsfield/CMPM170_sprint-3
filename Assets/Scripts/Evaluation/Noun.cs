using System.Collections.Generic;

public enum NounTag {
	DIRT,
	CORPSE,
}

public class NounInstance {
    
	private readonly HashSet<NounTag> _tags;

	public NounInstance(List<NounTag> tags) {
		_tags = new(tags);
	}

	public bool IsSubsetOf(HashSet<NounTag> tagSuperset) {
		return _tags.IsSubsetOf(tagSuperset);
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

