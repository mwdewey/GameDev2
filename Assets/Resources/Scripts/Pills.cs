using UnityEngine;
using System.Collections;

public class Pills : Item {
	public AudioClip dat_healing_sound;

	public override void Activate(){
		holder.GetComponent<PlayerController> ().held_item = null;
		holder.GetComponent<Consciousness> ().Revive ();
		holder.GetComponent<AudioSource> ().clip = dat_healing_sound;
		holder.GetComponent<AudioSource> ().Play ();
		Destroy (gameObject);
	}
}
