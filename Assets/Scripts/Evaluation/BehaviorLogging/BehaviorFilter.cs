using System.Collections.Generic;

public abstract class BehaviorFilter {

	public abstract BehaviorLog Filter(List<Behavior> behaviors);

}

public class SubjectFilter : BehaviorFilter {

	// TODO
	public SubjectFilter(NounRestriction criteria) {}

	// TODO
	public override BehaviorLog Filter(List<Behavior> behaviors) {
		return null;
	}

}

public class ObjectFilter : BehaviorFilter {

	// TODO
	public ObjectFilter(NounRestriction criteria) {}

	// TODO
	public override BehaviorLog Filter(List<Behavior> behaviors) {
		return null;
	}

}

public class VerbFilter : BehaviorFilter {

	// TODO
	public VerbFilter(VerbRestriction criteria) {}

	// TODO
	public override BehaviorLog Filter(List<Behavior> behaviors) {
		return null;
	}

}

