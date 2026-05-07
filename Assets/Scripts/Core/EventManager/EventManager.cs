using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager {
    /* Event-invoke function pairs are declared here.
     * The wrapper functions makes it so that we can invoke an event from any
     * script. All the events being in this manager also makes it so that any
     * suscribers only need to know about this event manager */

    // Triggered when an item is grabbed
    public static event Action grabToggled;
    public static void invokeGrabToggled() {
        grabToggled?.Invoke();
    }

    // Triggered when an item collides with something
    public static event Action<AudioClip, GameObject> itemCollided;
    public static void invokeItemCollided(AudioClip clip, GameObject obj) {
        itemCollided?.Invoke(clip, obj);
    }

	public static void InvokeTestEvent() {
		Debug.Log("drool");
	}

	public static event Action<NounInstance, NounInstance, VerbInstance> onBehavior;
	public static void InvokeItemGrabbed(NounInstance sub, NounInstance obj) {
		onBehavior?.Invoke(sub, obj, new(VerbType.DROPS, new()));
		Debug.Log("who up emitting");
	}

	public static void InvokeItemDropped(NounInstance sub, NounInstance obj) {
		onBehavior.Invoke(sub, obj, new VerbInstance(VerbType.DROPS, new()));
	}

	public static event Action<string> onBehaviorComplete;
	public static void InvokeBehaviorComplete(string pastTense) {
		Debug.Log(pastTense);
	}

    public static void Testing() {
        Debug.Log("Suscribers: " + itemCollided);
    }
}
