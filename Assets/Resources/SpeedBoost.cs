using UnityEngine;
using System.Collections;

public class SpeedBoost : Item {

	float active_lifetime;
	Transform t;
	Transform holder_t;
	bool dontSpam = true;
	public AudioClip speed_sound; 


	public override void Activate (){
		GetComponent<ParticleSystem> ().Play ();
		GetComponent<AudioSource> ().clip = speed_sound;
		GetComponent<AudioSource> ().Play ();
		holder.GetComponent<PlayerController> ().held_item = null; 	
		holder.GetComponent<PlayerController> ().speed_boost = 1.5f;
		active_lifetime = 20f;
		activated = true;
		t = GetComponent<Transform> ();
		holder_t = holder.GetComponent<Transform> ();
		rendy.enabled = true;
	}

	void Update(){
		if (activated) {

            if (t==null || holder==null || holder_t == null) {
                Destroy(gameObject);
                return;
            }

			active_lifetime -= Time.deltaTime;
			t.position = holder_t.position;
			if (active_lifetime <= 0) {
				GetComponent<ParticleSystem> ().Stop ();
				holder.GetComponent<PlayerController> ().speed_boost = 1.0f;
				Destroy (gameObject);
			}
		}
	}
}
