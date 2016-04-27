using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flashbang : Item {

	Transform t;
	int direction;
	Vector3 velocity;
	bool blinding;
	List<SpriteRenderer> blinders;
	float flash_duration;
	public Sprite blam;
	public AudioClip flashbang_sound;

	public override void Activate(){
		holder.GetComponent<PlayerController> ().held_item = null;
		GetComponent<ParticleTrail> ().Activate (holder);
		activated = true;
		blinding = false;
		t = GetComponent<Transform> ();
		direction = holder.GetComponent<Animator> ().GetInteger ("DirectionState");
		t.position = holder.transform.position;
		flash_duration = 2f;
		blinders = new List<SpriteRenderer> ();
		rendy.enabled = true;
		switch (direction)
		{
		case 0: 
			velocity = new Vector3 (-0.3f, 0.0f, 0.0f);
			break; // Left
		case 1: 
			velocity = new Vector3 (0.3f, 0.0f, 0.0f);
			break; // Right
		case 2: 
			velocity = new Vector3 (0.0f, 0.3f, 0.0f);
			break; // Up
		case 3: 
			velocity = new Vector3 (0.0f, -0.3f, 0.0f);
			break; // Down
		}

        Vector2 player_velocity = holder.GetComponent<Rigidbody2D>().velocity;
        velocity.x += player_velocity.x / 60;
        velocity.y += player_velocity.y / 60;
		collidy.enabled = true;
		collidy.radius = 1;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (activated) {
			if ((other.tag == "PlayerObject" && other.gameObject != holder) || other.tag == "Wall") {
				SetOff ();
			}
		} 
		else {
			if (other.tag == "PlayerObject") {
				other.GetComponent<PlayerController> ().item_list.Add (this);
				ring.enabled = true;
			}
		}
	}

	void SetOff(){
		GetComponent<ParticleTrail> ().Deactivate ();
		GetComponent<AudioSource> ().clip = flashbang_sound;
		GetComponent<AudioSource> ().Play();
		rendy.sprite = blam;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("PlayerObject");
		foreach (GameObject player in players) {
			if ((player.transform.position - t.position).magnitude < 2f && player != holder) {
				blinding = true;
				GameObject blinder = (GameObject)Instantiate (Resources.Load ("Prefabs/Environment/Blinder"), player.transform.position, Quaternion.identity);
				blinder.layer = 9 + int.Parse (player.GetComponent<PlayerController> ().PID);
				blinders.Add (blinder.GetComponent<SpriteRenderer>());
			}
		}
		if (!blinding) {
			blinding = true;
			StartCoroutine (Fizzle ());
			return;
		}
		StartCoroutine (Flasher());
	}

	IEnumerator Fizzle(){
		yield return new WaitForSeconds (0.1f);
		Destroy (gameObject);
	}

	IEnumerator Flasher(){
		yield return new WaitForSeconds (0.1f);
		rendy.enabled = true;
		yield return new WaitForSeconds (flash_duration - 0.6f);
		flash_duration = 0.5f;
		while(true){
			yield return new WaitForEndOfFrame ();
			flash_duration -= Time.deltaTime;
			foreach (SpriteRenderer blinder in blinders) {
				blinder.color = new Color(blinder.color.r, blinder.color.g, blinder.color.g, flash_duration * 2);
			}
			if (flash_duration <= 0) {
				foreach (SpriteRenderer blinder in blinders) {
					Destroy (blinder.gameObject);
				}
				Destroy (gameObject);
				break;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (activated && !blinding) {
			t.position += velocity;
		}
	}
}
