﻿using UnityEngine;
using System.Collections;

public class BowlingBall : Item {

	public float speed;
	int direction;
	Vector3 velocity;
	Transform t;
	public GameObject blam;


	public override void Activate(){
		holder.GetComponent<PlayerController> ().held_item = null;
		activated = true;
		direction = holder.GetComponent<Animator> ().GetInteger ("DirectionState");
		t = GetComponent<Transform> ();
		rendy.enabled = true;
		collidy.enabled = true;
		collidy.radius = 4f;
		t.position = holder.transform.position - new Vector3(0f, 0.3f, 0f);
		switch (direction)
		{
		case 0: 
			velocity = new Vector3 (-speed, 0.0f, 0.0f);
			break; // Left
		case 1: 
			velocity = new Vector3 (speed, 0.0f, 0.0f);
			break; // Right
		case 2: 
			velocity = new Vector3 (0.0f, speed, 0.0f);
			break; // Up
		case 3: 
			velocity = new Vector3 (0.0f, speed, 0.0f);
			break; // Down
		}

		Vector2 player_velocity = holder.GetComponent<Rigidbody2D>().velocity;
		velocity.x += player_velocity.x / 60;
		velocity.y += player_velocity.y / 60;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (activated){
			if (other.tag == "PlayerObject" && other.gameObject != holder && !other.GetComponent<PlayerController>().unconscious) {
				other.gameObject.SendMessage ("TakeDamage", 50);
				Instantiate (blam, t.position, Quaternion.identity);
			} 
			else if (other.tag == "Wall") {
				Destroy (gameObject);
				Instantiate (blam, t.position, Quaternion.identity);
			}
		}
		else if (other.tag == "PlayerObject") {
			other.GetComponent<PlayerController> ().item_list.Add (this);
			ring.enabled = true;
		}
	}

	void Update(){
		if (activated) {
			t.position += velocity;
		}
	}
}