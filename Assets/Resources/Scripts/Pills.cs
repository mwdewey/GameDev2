using UnityEngine;
using System.Collections;

public class Pills : Item {
	public AudioClip dat_healing_sound;
	public GameObject particles;

	public override void Activate(){
		holder.GetComponent<PlayerController> ().held_item = null;
		holder.GetComponent<Consciousness> ().Revive ();
		GameObject x = (GameObject) Instantiate(particles, holder.transform.position, Quaternion.identity);
		x.transform.SetParent (holder.transform);
		x.GetComponent<AudioSource> ().clip = dat_healing_sound;
		x.GetComponent<AudioSource> ().Play ();
		Destroy (gameObject);
	}
}
