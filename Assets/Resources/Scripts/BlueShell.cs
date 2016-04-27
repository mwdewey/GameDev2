using UnityEngine;
using System.Collections;

public class BlueShell : Item {

	GameObject target;
	Transform t;
	Vector3 velocity;
	float speed;
	public AudioClip blue_shell_sound;
	public ParticleSystem blue_particles;
	
	public override void Activate(){
		holder.GetComponent<PlayerController> ().held_item = null;
		GetComponent<ParticleTrail> ().Activate (holder);
		t = GetComponent<Transform> ();
		t.position = holder.transform.position;
		//Debug.Log ("YOU HAVE SENT OUT A BLUE SHELL");
		activated = true;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("PlayerObject");
		int highest_score = -1;
		target = null;
		foreach (GameObject person in players){
            if (person == holder || person == null || person.GetComponent<Score_Counter>() == null)
            {
				continue;
			}
			//Debug.Log (highest_score + " " + target + " " + person.GetComponent<Score_Counter>().score);
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
		}
	}

	Vector3 DynamicSeek() {
		Vector3 desired_velocity = target.transform.position - t.position;
		float target_distance = desired_velocity.magnitude;
		desired_velocity.Normalize ();
		desired_velocity *= speed;

		if (target_distance < 0.3f) {
            target.GetComponent<Consciousness>().TakeDamage(50, holder.GetComponent<PlayerController>().PID);
			//Debug.Log ("BOOM HEADSHOT");
			target.GetComponent<AudioSource>().clip = blue_shell_sound;
			target.GetComponent<AudioSource>().Play ();
			Instantiate (blue_particles, transform.position, Quaternion.identity);
			activated = false;

            // update damage stats
            PlayerStats.getStats(holder.GetComponent<PlayerController>().PID).damageDone += 50;
            PlayerStats.getStats(target.gameObject.GetComponent<PlayerController>().PID).damageReceived += 50;

			Destroy (gameObject);
		}

		Vector3 steering = desired_velocity - velocity;
		steering = Vector3.ClampMagnitude (steering, 0.005f);

		return steering;
	}

	void Update(){
		if (activated && target != null) {
			velocity += DynamicSeek ();
			velocity.z = 0;
			t.position += velocity;
		}
	}

}
