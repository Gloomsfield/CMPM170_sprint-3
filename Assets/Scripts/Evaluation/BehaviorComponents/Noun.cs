using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

/// <summary>
/// The <c>NounTag</c> enum defines the possible tags that a
/// <see cref="NounInstance"/> can have.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum NounTag {
	PLAYER,
	DIRT,
	CORPSE,
	COOKABLE,
	FIRE,
	ANY,
}

/// <summary>
/// The NounInstance class is constructed from a name and a list
/// of tags. NounInstances should mainly just be passed as
/// parameters.
/// <summary>
public class NounInstance {
    
	private readonly HashSet<NounTag> _tags;

	public string name;

	/// <summary>
	/// The <c>NounInstance</c> constructor builds a <c>NounInstance</c> from a
	/// <paramref name="name"/> and a <paramref name="tags">list of
	/// tags</paramref>.
	/// </summary>
	/// <param name="name">The name to associate with this NounInstance.</param>
	/// <param name="tags">The tags with which this NounInstance will be
	/// associated. These are used to determine whether this NounInstance fits
	/// within a <see cref="Pattern"/>.</param>
	public NounInstance(string name, List<NounTag> tags) {
		this.name = name;
		_tags = new(tags);
	}

	/// <summary>
	/// The member function <c>IsSubsetOf</c> determines whether a
	/// <c>NounInstance</c>'s tags are completely contained within
	/// <paramref name="tagSuperset">a set of</paramref>
	/// <see cref="NounTag">NounTags</see>. This really shouldn't
	/// be called outside of the evaluation system.
	/// </summary>
	/// <param name="tagSuperset">The set of tags to check
	/// against.</param>
	public bool IsSubsetOf(HashSet<NounTag> tagSuperset) {
		return tagSuperset.Contains(NounTag.ANY) || _tags.IsSubsetOf(tagSuperset);
	}

}

/// <summary>
/// The <c>NounRestriction</c> class exists to ensure that
/// a <see cref="NounInstance"/> conforms to a set of
/// tags. This class really shouldn't be instantiated outside
/// of the evaluation system.
/// </summary>
public class NounRestriction {
	
	private readonly HashSet<NounTag> _validTags;

	/// <summary>
	/// The <c>NounRestriction</c> constructor builds a
	/// <c>NounRestriction</c> from a <paramref name="tags">list
	/// of tags</paramref>.
	/// </summary>
	/// <param name="tags">A <c>List</c> of
	/// <see cref="NounTag">NounTags</see> that determines this
	/// <c>NounRestriction.<c></param>
	public NounRestriction(List<NounTag> tags) {
		_validTags = new(tags);
	}

	/// <summary>
	/// The <c>CheckConformity</c> method determines whether
	/// a <see cref="NounInstance"/> conforms to this
	/// <c>NounRestriction</c>. I.e., "Are all of the
	/// <see cref="NounInstance"/>'s tags allowed by this
	/// <c>NounRestriction</c>?"
	public bool CheckConformity(NounInstance noun) {
		return noun.IsSubsetOf(_validTags);
	}

}

/// <summary>
/// The <c>NounRestrictionBlueprint</c> class exists to
/// build <see cref="NounRestriction">NounRestrictions</see>
/// from lists of <see "NounTag">NounTags</see>. This class
/// should really only be constructed by the evaluation
/// system.
/// </summary>
public class NounRestrictionBlueprint {
	
	[JsonProperty("identifier")]
	public readonly string identifier;

	[JsonProperty("validTags")]
	private readonly List<NounTag> _validTags;

	/// <summary>
	/// The <c>Build</c> member function returns a newly
	/// constructed <see cref="NounRestriction"/> instance.
	/// This should really only be called by the evaluation
	/// system.
	/// <summary>
	public NounRestriction Build() {
		return new NounRestriction(_validTags);
	}

}

