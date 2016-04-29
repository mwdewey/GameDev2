using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cage : MonoBehaviour {

	bool active;
	bool locked;
	float locked_timer;
	PlayerController victim;
	public Sprite unlocked_sprite;
	public AudioClip cage_sound;
	private Vector2 cage_top_left;
	private float distance_to_box;

	public GameObject coin;

	int coins_to_eject=10;
	int coins_left = 0;

	// Use this for initialization
	void Start () {
		active = true;
		locked = false;
		locked_timer = 5;

		Vector2 prospective_pos = transform.Find("Locked Door").position;
		cage_top_left = new Vector2(transform.position.x - GetComponent<SpriteRenderer> ().bounds.size.x/2,
			transform.position.y- GetComponent<SpriteRenderer>().bounds.size.y/2);
		distance_to_box = Vector2.Distance (transform.position, transform.Find("Locked Door").position);
		if (IsGoodPlaceForBox (prospective_pos)) {
			return;
		} else if (IsGoodPlaceForBox (new Vector2 (transform.position.x, transform.position.y + distance_to_box))) {
			transform.Find("Locked Door").position = new Vector2 (transform.position.x, transform.position.y + distance_to_box);
			return;
		} else if (IsGoodPlaceForBox (new Vector2 (transform.position.x - distance_to_box, transform.position.y))) {
			transform.Find("Locked Door").position = new Vector2 (transform.position.x - distance_to_box, transform.position.y);
			return;
		} else if (IsGoodPlaceForBox (new Vector2 (transform.position.x, transform.position.y - distance_to_box))) {
			transform.Find("Locked Door").position = new Vector2 (transform.position.x, transform.position.y - distance_to_box);
			return;
		} else {
			//there's nowhere nearby to put this box. Kill the object
			Destroy (gameObject);
			return;
		}
	}

	bool IsGoodPlaceForBox(Vector2 prospective_pos){

		Collider2D[] colliders = Physics2D.OverlapAreaAll (cage_top_left, prospective_pos);
		if (colliders.Length == 0) 
			return true;
		
		foreach (Collider2D c in colliders){
			if (c.gameObject.tag == "Wall")
				return false;
		}

		return true;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (!active || locked) {
			return;
		}
		if (other.tag == "PlayerObject") {
			//Debug.Log ("MWAHAHA, YOU HAVE BEEN TRAPPED, MORTAL");
			locked = true;
			victim = other.GetComponent<PlayerController> ();
			victim.unconscious = true;
			victim.setKnockBack (null);
			victim.locked = true;
			other.transform.position = transform.position;
			GetComponentsInChildren<SpriteRenderer>()[1].sprite = unlocked_sprite;
			GetComponent<AudioSource> ().clip = cage_sound;
			GetComponent<AudioSource> ().Play ();
			coins_left = coins_to_eject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (locked && active) {
			locked_timer -= Time.deltaTime;
			if (locked_timer <= 0) {
				//Debug.Log ("Okay, you can go now. I got bored");
				locked = false;
				active = false;
				victim.locked = false;
				victim.unconscious = false;
			}
		}
		if (coins_left>0) {
			GetComponentInChildren<FountainObjFlight> ().SpawnObject (coin);
			coins_left--;
			Manager.coins_remaining += 1;
		}
	}
}
