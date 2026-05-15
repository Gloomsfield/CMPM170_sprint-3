using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager {
    /* Event-invoke function pairs are declared here.
     * The wrapper functions makes it so that we can invoke an event from any
     * script. All the events being in this manager also makes it so that any
     * suscribers only need to know about this event manager */

    // Triggered when an item is grabbed
    public static event Action grabStart;
    public static void InvokeGrabStart() {
        grabStart?.Invoke();
    }

	public static event Action grabEnd;
	public static void InvokeGrabEnd() {
		grabEnd?.Invoke();
	}

    // Triggered when an item collides with something
    public static event Action<AudioClip, GameObject, AudioSourceConfig> itemCollided;
    public static void InvokeItemCollided(AudioClip clip, GameObject obj, AudioSourceConfig config) {
        itemCollided?.Invoke(clip, obj,config);
    }

	public static void InvokePatternsGenerated(string p) {
		Debug.Log(p);
	}

	public static event Action<Behavior> onBehavior;
	public static void InvokeItemGrabbed(NounInstance sub, NounInstance obj) {
		onBehavior?.Invoke(new(sub, obj, new(VerbType.GRABS, new())));
	}

	public static void InvokeItemDropped(NounInstance sub, NounInstance obj) {
		onBehavior.Invoke(new(sub, obj, new VerbInstance(VerbType.DROPS, new())));
	}

	public static void InvokeDebug(string d) {
		Debug.Log(d);
	}

	public static event Action<string> onBehaviorComplete;
	public static void InvokeBehaviorComplete(Behavior behavior) {
		Debug.Log($"The {behavior.sub.name} {behavior.verb.Conjugate(VerbTense.PAST)} the {behavior.obj.name}");
	}

    // TODO
    public static void Testing() {
        Debug.Log("Suscribers: " + itemCollided);
    }

    public static event Action therapyStarted;
    public static void InvokeTherapyStarted() {
        therapyStarted?.Invoke();
    }

    public static event Action therapyEnded;
    public static void InvokeTherapyEnded() {
        therapyEnded?.Invoke();
    }

    public static event Action IntroSceneComplete;
    public static void InvokeIntroSceneComplete() {
        IntroSceneComplete?.Invoke();
    }

    public static event Action playtestEnded;
    public static void invokePlaytestEnded() {
        playtestEnded?.Invoke();
    }

    // Special action is reserved for any action that uses the right MB
    public static event Action specialAction;
    public static void InvokeSpecialAction() {
        specialAction?.Invoke();
    }

}
