using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flashbang : Item {

	Transform t;
	int direction;
	Vector3 target;
	Vector3 velocity;
	bool blinding;
	List<SpriteRenderer> blinders;
	float flash_duration;
	public Sprite blam;
	public AudioClip flashbang_sound;

	public override void Activate(){
		holder.GetComponent<PlayerController> ().held_item = null;
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
			target = t.position + new Vector3 (-3.0f, 0.0f, 0.0f);  
			velocity = new Vector3 (-0.3f, 0.0f, 0.0f);
			break; // Left
		case 1: 
			target = t.position + new Vector3 (3.0f, 0.0f, 0.0f);
			velocity = new Vector3 (0.3f, 0.0f, 0.0f);
			break; // Right
		case 2: 
			target = t.position + new Vector3 (0.0f, 3.0f, 0.0f);  
			velocity = new Vector3 (0.0f, 0.3f, 0.0f);
			break; // Up
		case 3: 
			target = t.position + new Vector3 (0.0f, -3.0f, 0.0f); 
			velocity = new Vector3 (0.0f, -0.3f, 0.0f);
			break; // Down
		}

        Vector2 player_velocity = holder.GetComponent<Rigidbody2D>().velocity;
        velocity.x += player_velocity.x / 60;
        velocity.y += player_velocity.y / 60;

        print(velocity.x + " " + velocity.y);

		GetComponent<ParticleTrail>().addParticleTrails(holder);
	}
	
	// Update is called once per frame
	void Update () {
		if (activated) {
			if (!blinding) {
				t.position += velocity;
				if ((t.position - target).magnitude <= 0.1f) {
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
						Destroy (gameObject);
					}
				}
			} 
			else {
				flash_duration -= Time.deltaTime;
				if (flash_duration <= 1.9f && rendy.enabled) {
					rendy.enabled = false;
				} 
				else if (flash_duration <= 0.5f) {
					foreach (SpriteRenderer blinder in blinders) {
						blinder.color = new Color(blinder.color.r, blinder.color.g, blinder.color.g, flash_duration * 2);
					}
				}
				else if (flash_duration <= 0) {
					foreach (SpriteRenderer blinder in blinders) {
						Destroy (blinder.gameObject);
					}
					Destroy (gameObject);
				}
			}
		}
	}
}
