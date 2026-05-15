using UnityEngine;

/* This class is responsible for spreading fire to flammable object */
public class FireMod : MonoBehaviour {
    [SerializeField] GameObject fire;
    [SerializeField] int duration = 5;

    private void OnCollisionEnter(Collision other) {
        /* You can get components by what interface they implement, so this
         * gets a ref to the first script attached to other which implements Iflammable*/
        Iflammable flammable = other.gameObject.GetComponent<Iflammable>();
        if (flammable == null) return;
        flammable.StartFire(duration);
    }
}
