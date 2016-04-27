using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlueShell : Item {

	GameObject target;
	List<GameObject> targets;
	Transform t;
	Vector3 velocity;
	float speed;
	public AudioClip blue_shell_sound;
	public ParticleSystem blue_particles;
	
	public override void Activate(){
		holder.GetComponent<PlayerController> ().held_item = null;
		GetComponent<ParticleTrail>().Activate (holder);
		t = GetComponent<Transform> ();
		t.position = holder.transform.position;
		activated = true;
		int highest_score = -1;
		target = null;
		targets = new List<GameObject> (GameObject.FindGameObjectsWithTag ("PlayerObject"));
		targets.Remove (holder);
		foreach (GameObject person in targets){
            if (person == holder || person == null || person.GetComponent<Score_Counter>() == null)
            {
				continue;
			}
			if (target == null || person.GetComponent<Score_Counter> ().score > highest_score) {
				highest_score = person.GetComponent<Score_Counter> ().score;
				target = person;
			}
			else if (person.GetComponent<Score_Counter>().score == highest_score && (person.transform.position - t.position).sqrMagnitude < (target.transform.position - t.position).sqrMagnitude){
				target = person;
			}
		}
		if (target == null) {
			//Debug.Log ("FOUND NO TARGET, SELF DESTRUCTING");
			activated = false;
			Destroy (gameObject);
		} 
		else {
			velocity = Vector3.zero;
			rendy.enabled = true;
			speed = 0.08f;
			collidy.enabled = true;
			collidy.radius = 3f;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (activated) {
			if (other.tag == "PlayerObject" && other.gameObject != holder) {
				GameObject[] players = GameObject.FindGameObjectsWithTag ("PlayerObject");
				foreach (GameObject player in players) {
					if ((player.transform.position - t.position).magnitude < 2f && player != holder) {
						player.GetComponent<Consciousness> ().TakeDamage (50, holder.GetComponent<PlayerController> ().PID);
					}
				}

				target.GetComponent<AudioSource> ().clip = blue_shell_sound;
				target.GetComponent<AudioSource> ().Play ();
				Instantiate (blue_particles, transform.position, Quaternion.identity);

				// update damage stats
				PlayerStats.getStats (holder.GetComponent<PlayerController> ().PID).damageDone += 50;
				PlayerStats.getStats (target.gameObject.GetComponent<PlayerController> ().PID).damageReceived += 50;

				Destroy (gameObject);
			}
		}
		else if (other.tag == "PlayerObject") {
			other.GetComponent<PlayerController> ().item_list.Add (this);
			ring.enabled = true;
		}
	}

	Vector3 DynamicSeek() {
		Vector3 desired_velocity = target.transform.position - t.position;
		float target_distance = desired_velocity.magnitude;
		desired_velocity.Normalize ();
		desired_velocity *= speed;
		Vector3 steering = desired_velocity - velocity;
		steering = Vector3.ClampMagnitude (steering, 0.005f);

		return steering;
	}

	void Update(){
		if (activated && target != null) {
			foreach (GameObject person in targets) {
				if (person.GetComponent<Score_Counter> ().score > target.GetComponent<Score_Counter> ().score) {
					target = person;
				}
			}
			velocity += DynamicSeek ();
			velocity.z = 0;
			t.position += velocity;
		}
	}

}
