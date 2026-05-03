using UnityEngine;

public class GrabController : MonoBehaviour, IsGrabbable
{
    private bool grabbed = false;

    public void ToggleGrab() {
        grabbed = !grabbed;
        if (grabbed) {
            transform.SetParent(Camera.main.transform);
        } else {
            transform.SetParent(null);
        }

    }
}
