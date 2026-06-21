using UnityEngine;
using System.Collections.Generic;

public class BehaviorLog {
	
	private List<Behavior> _log = new();

	// TODO
	public BehaviorLog() {
        EventManager.onBehavior += Log;
    }

	// TODO
	public BehaviorLog Filter(BehaviorFilter filter) {
		return null;
	}

    public void Log(Behavior behavior) {
        return;
        string s = behavior.ToString();
        Debug.Log(s);
    }
}

