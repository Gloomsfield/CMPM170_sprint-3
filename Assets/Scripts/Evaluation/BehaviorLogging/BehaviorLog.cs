using UnityEngine;
using System.Collections.Generic;

public class BehaviorLog {
	
	private List<Behavior> _log = new();

	// TODO
	public BehaviorLog() {
        // <Max Kinet>
        EventManager.onBehavior += Log;
    }

	// TODO
	public BehaviorLog Filter(BehaviorFilter filter) {
		return null;
	}

    // MAX KINET'S ADDITIONS ---------------------------------------------------
    public void Log(Behavior behavior) {
        behavior.ToString();
    }

    // END MAX KINET'S ADDITIONS

}

