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
		EventManager.InvokeBehavior(new(playerNoun, noun, new VerbInstance(VerbType.GRABS, new())));
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
