using System.Collections;
using UnityEngine;

/* This class is called on when the attached object is set on fire. It describes
 * what will happen when a fire is started. Note that an object must have a script
 * that implements Iflammable to be set on fire */
public class FireController : MonoBehaviour, Iflammable {
    [SerializeField] GameObject fireVfx;
    private bool onFire = false;

    public void StartFire(int duration) {
        if (onFire) return;
        onFire = true;
        fireVfx.SetActive(true);
        StartCoroutine(Burn(duration));
    }

    IEnumerator Burn(int duration) {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
