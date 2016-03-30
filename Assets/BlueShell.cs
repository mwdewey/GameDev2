using UnityEngine;
using System.Collections;

public class BlueShell : Item {

	GameObject target;
	Transform t;
	Vector3 velocity;
	float speed;
	
	public override void Activate(){
		t = GetComponent<Transform> ();
		t.position = holder.transform.position;
		Debug.Log ("YOU HAVE SENT OUT A BLUE SHELL");
		activated = true;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("PlayerObject");
		int highest_score = -1;
		target = null;
		foreach (GameObject person in players){
			if (person == holder) {
				continue;
			}
			Debug.Log (highest_score + " " + target + " " + person.GetComponent<movement>().score);
			if (target == null || person.GetComponent<movement> ().score > highest_score) {
				highest_score = person.GetComponent<movement> ().score;
				target = person;
			}
			else if (person.GetComponent<movement>().score == highest_score && (person.transform.position - t.position).sqrMagnitude < (target.transform.position - t.position).sqrMagnitude){
				target = person;
			}
		}
		if (target == null) {
			Debug.Log ("FOUND NO TARGET, SELF DESTRUCTING");
			activated = false;
			Destroy (gameObject);
		} 
		else {
			velocity = Vector3.zero;
			rendy.enabled = true;
			speed = 0.1f;
		}
	}

	Vector3 DynamicSeek() {
		Vector3 desired_velocity = target.transform.position - t.position;
		float target_distance = desired_velocity.magnitude;
		desired_velocity.Normalize ();
		desired_velocity *= speed;

		if (target_distance < 0.3f) {
			target.GetComponent<movement> ().TakeDamage (3);
			Debug.Log ("BOOM HEADSHOT");
			activated = false;
			Destroy (gameObject);
		}

		Vector3 steering = desired_velocity - velocity;
		steering = Vector3.ClampMagnitude (steering, 0.005f);

		return steering;
	}

	void Update(){
		if (activated) {
			velocity += DynamicSeek ();
			velocity.z = 0;
			t.position += velocity;
		}
	}

}
