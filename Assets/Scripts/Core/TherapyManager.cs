using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class TherapyManager : MonoBehaviour {
    public static TherapyManager Instance { get; private set; }

    //EVENTS
    /* EX This is how you declare an event. Action is a delegate, which you can think of as a function signature.
     * Actions return void, and will have whatever parameters you specify in the <>.
     * When we declare an event as Action<float>, we are saying that when invoked, it can call subscribers of the form
     * void Function (float f). It is important that anything subscribing to this function of this form. */
    static public event Action<float> therapyStarted;

    void Awake() {
        // Self implode if a TherapyManager already exists
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //TO REMOVE
        // Instatiate GameManager for the example
        GameManager.Instance.accessed = true;
    }

    void Update() {
        // EXAMPLE EVENT
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            /*EX this is how we invoke an event. The question mark is there to make sure
             * the event is not null before we invoke it. Null events are those with no 
             * suscribers.
             * This will call all of the suscribed functions with the passed parameter.*/
            therapyStarted?.Invoke(Time.time);
        }
    }

}
    
