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

	public GameObject coin;

	int coins_to_eject=10;
	int coins_left = 0;

	// Use this for initialization
	void Start () {
		active = true;
		locked = false;
		locked_timer = 5;
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
