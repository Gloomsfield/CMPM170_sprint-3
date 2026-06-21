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

    // <Max Kinet> ---------------------------------------------------------
    private void OnCollisionEnter(Collision other) {
        other.gameObject.GetComponent<ItemNounWrapper>()?.OnTouch(gameObject); 
    }
    // <End Max Kinet> ------------------------------------------------------

	public void OnGrab() {
		NounInstance playerNoun = GameObject.FindWithTag("Player").GetComponent<ItemNounWrapper>().noun;
		EventManager.InvokeBehavior(new(playerNoun, noun, new VerbInstance(VerbType.GRABS, new())));
	}

    public void OnIgnite(GameObject obj) {
		NounInstance noun = obj.GetComponent<ItemNounWrapper>().noun;
		EventManager.InvokeBehavior(new(noun, this.noun, new VerbInstance(VerbType.IGNITES, new())));
    }

    public void OnTouch(GameObject obj) {
		NounInstance noun = obj.GetComponent<ItemNounWrapper>().noun;
		EventManager.InvokeBehavior(new(this.noun, noun, new VerbInstance(VerbType.TOUCHES, new())));
    }
	public void OnDrop() {
		NounInstance playerNoun = GameObject.FindWithTag("Player").GetComponent<ItemNounWrapper>().noun;
		EventManager.InvokeBehavior(new(playerNoun, noun, new VerbInstance(VerbType.DROPS, new())));
	}
	
	public void OnThrow() {
		NounInstance playerNoun = GameObject.FindWithTag("Player").GetComponent<ItemNounWrapper>().noun;
		EventManager.InvokeBehavior(new(playerNoun, noun, new VerbInstance(VerbType.THROWS, new())));
	}

}
