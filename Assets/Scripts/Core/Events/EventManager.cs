using System;
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
    public static event Action<AudioClip, GameObject,AudioSourceConfig> itemCollided;
    public static void invokeItemCollided(AudioClip clip, GameObject obj, AudioSourceConfig config) {
        itemCollided?.Invoke(clip, obj,config);
    }

    // TODO
    public static void Testing() {
        Debug.Log("Suscribers: " + itemCollided);
    }

    public static event Action therapyStarted;
    public static void invokeTherapyStarted() {
        therapyStarted?.Invoke();
    }

    public static event Action therapyEnded;
    public static void invokeTherapyEnded() {
        therapyEnded?.Invoke();
    }

    public static event Action playtestEnded;
    public static void invokePlaytestEnded() {
        playtestEnded?.Invoke();
    }
}
