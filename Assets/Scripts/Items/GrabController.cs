using UnityEngine;

public class GrabController : MonoBehaviour, IsGrabbable
{
    public void Grab() {
        Debug.Log("Test object was grabbed");
    }
}
