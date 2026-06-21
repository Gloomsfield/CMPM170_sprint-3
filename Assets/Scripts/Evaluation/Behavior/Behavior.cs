
public class Behavior {
	
	public NounInstance sub;
	public NounInstance obj;
	public VerbInstance verb;

	public Behavior(NounInstance sub, NounInstance obj, VerbInstance verb) {
		this.sub = sub;
		this.obj = obj;
		this.verb = verb;
	}

    // <Max Kinet> ---------------------------
    override public string ToString() {
        return $"{sub.name} {verb.Conjugate(VerbTenseType.SIMPLE_PRESENT)} {obj.name}";
    }
    // <End Max Kinet> -----------------------

}

