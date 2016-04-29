using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BowlingBall : Item {

	public float speed;
	int direction;
	Vector3 velocity;
	Transform t;
	public GameObject blam;
	public List<Sprite> vertical;
	public List<Sprite> horizontal;
	List<Sprite> which_sprite;
	int anim;


	public override void Activate(){
		holder.GetComponent<PlayerController> ().held_item = null;
		GetComponent<ParticleTrail> ().Activate (holder);
		activated = true;
		direction = holder.GetComponent<Animator> ().GetInteger ("DirectionState");
		t = GetComponent<Transform> ();
		rendy.enabled = true;
		collidy.enabled = true;
		collidy.radius = 0.2f;
		t.position = holder.transform.position - new Vector3(0f, 0.3f, 0f);
		switch (direction)
		{
		case 0: 
			velocity = new Vector3 (-speed, 0.0f, 0.0f);
			which_sprite = horizontal;
			break; // Left
		case 1: 
			velocity = new Vector3 (speed, 0.0f, 0.0f);
			which_sprite = horizontal;
			break; // Right
		case 2: 
			velocity = new Vector3 (0.0f, speed, 0.0f);
			which_sprite = vertical;
			break; // Up
		case 3: 
			velocity = new Vector3 (0.0f, -speed, 0.0f);
			which_sprite = vertical;
			break; // Down
		}
		Vector2 player_velocity = holder.GetComponent<Rigidbody2D>().velocity;
		velocity.x += player_velocity.x / 60;
		velocity.y += player_velocity.y / 60;
		rendy.sprite = which_sprite [0];
		anim = 0;
		StartCoroutine (Roll ());
	}

	IEnumerator Roll(){
		while (true) {
			yield return new WaitForSeconds (0.2f);
			anim++;
			if (anim >= 4) {
				anim = 0;
			}
			rendy.sprite = which_sprite [anim];
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (activated){
			if (other.tag == "PlayerObject" && other.gameObject != holder && !other.GetComponent<PlayerController>().unconscious) {
                other.GetComponent<Consciousness>().TakeDamage(50, holder.GetComponent<PlayerController>().PID);
				Instantiate (blam, t.position, Quaternion.identity);

                // update damage stats
                PlayerStats.getStats(holder.GetComponent<PlayerController>().PID).damageDone += 50;
                PlayerStats.getStats(other.gameObject.gameObject.GetComponent<PlayerController>().PID).damageReceived += 50;
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
			//transform.Rotate (0, 0, -10);
		}
	}
}
