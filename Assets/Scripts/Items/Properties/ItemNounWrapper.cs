using System.Collections.Generic;
using UnityEngine;

public class ItemNounWrapper : MonoBehaviour {

	public string nounInstanceName;

	public List<NounTag> nounTags;
	public NounInstance noun;

    void Start() {
		noun = new(nounInstanceName, nounTags);
    }

    void Update() {
        
    }

	public void OnGrab() {
		NounInstance playerNoun = GameObject.FindWithTag("Player").GetComponent<ItemNounWrapper>().noun;
		EventManager.InvokeItemGrabbed(playerNoun, noun);
	}

	public void OnDrop() {}
	
	public void OnThrow() {}

}
