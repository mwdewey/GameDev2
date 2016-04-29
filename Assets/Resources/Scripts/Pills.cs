using UnityEngine;
using System.Collections;

public class Pills : Item {

	public override void Activate(){
		holder.GetComponent<PlayerController> ().held_item = null;
		holder.GetComponent<Consciousness> ().Revive ();
		Destroy (gameObject);
	}
}
