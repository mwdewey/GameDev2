using UnityEngine;
using System.Collections;

public class SpeedBoost : Item {

	float active_lifetime;
	Transform t;
	Transform holder_t;
	bool dontSpam = true;
	public AudioClip speed_sound;
	PlayerController pc;


	public override void Activate (){
		ParticleSystem.EmissionModule emitter = GetComponent<ParticleSystem> ().emission;
		emitter.enabled = true;
		GetComponent<AudioSource> ().clip = speed_sound;
		GetComponent<AudioSource> ().Play ();
		pc = holder.GetComponent<PlayerController> ();
		pc.held_item = null; 	
		pc.speed_boost = 1.5f;
		active_lifetime = 20f;
		activated = true;
		t = GetComponent<Transform> ();
		holder_t = holder.GetComponent<Transform> ();
		t.position = holder_t.position;
		rendy.enabled = true;
	}

	void Update(){
		if (activated) {

			if (pc.speed_boost != 1.5f) {
				pc.speed_boost = 1.5f;
			}

            if (t==null || holder==null || holder_t == null) {
                Destroy(gameObject);
                return;
            }

			active_lifetime -= Time.deltaTime;
			t.position = holder_t.position;
			if (active_lifetime <= 0) {
				GetComponent<ParticleSystem> ().Stop ();
				pc.speed_boost = 1.0f;
				Destroy (gameObject);
			}
		}
	}
}
