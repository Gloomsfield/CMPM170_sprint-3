using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInputHandler : MonoBehaviour
{
    public bool ContinueTriggered { get; private set; } 

    // Automatically called by PLayerInput when space is pressed
    public void OnContinue(InputValue value)
    {
		EventManager.InvokeContinueTriggered();
    }

    // Resets space trigger at the end of frame to make sure it isnt called twice
    void LateUpdate()
    {
        ContinueTriggered = false;
    }
}
