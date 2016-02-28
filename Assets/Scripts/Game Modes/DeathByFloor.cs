﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeathByFloor : MonoBehaviour {
	/*Put this script on any floor to instantly make it
	 * A FLOOR OF DEATH!! You choose in this script how long 
	 * the player will take to respawn because this script is 
	 * in control of that too! OnTriggerEnter2D replaces the
	 * player with the Sprite dead (publicly assigned, see 
	 * below), and then starts a coroutine that counts down
	 * to the respawn and then makes it happen.
	*/

	int millis_until_respawn = 3000;
	int closest_distance;

	Sprite original_sprite; //so we can set the original sprite back after they aren't dead anymore
	public Sprite dead;		//this is the dead version of the player. Code can be added to pick which sprite that is later on

	void OnTriggerEnter2D(Collider2D c){
		if (c.gameObject.tag == "Player") {
			//You can do whatever you want to the player who just collided by using c.gameObject.whatever. 
			original_sprite = c.gameObject.GetComponent<SpriteRenderer>().sprite;
			  //save sprite info so it can be restored later
			c.gameObject.GetComponent<SpriteRenderer> ().sprite = dead;//switch sprite
			c.gameObject.GetComponent<PlayerMovement> ().enabled = false;//stop movement
			c.gameObject.GetComponent<Attacks> ().enabled = false;//stop attacks
			c.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			  //make sure they don't just keep moving on the velocity they already had
			StartCoroutine (RespawnCountdown( c.gameObject, original_sprite )); 
			  //countdown to respawn
		}
	}

	IEnumerator RespawnCountdown(GameObject g, Sprite original){
		yield return new WaitForSeconds(millis_until_respawn/1000f); 

		//pick the closest available respawn point
		List<GameObject> all_respawn_pts = GameObject.FindGameObjectsWithTag ("Respawn").ToList();
		GameObject closest_spawn_pt;
		for (int i = 0; i < all_respawn_pts.Count; i++) {
			float dis = Vector3.Distance (all_respawn_pts [i].transform.position, transform.position);
			if (i == 0 || dis < closest_distance) {
                closest_distance = (int) dis;
				closest_spawn_pt = all_respawn_pts [i];
			}
		}

		g.GetComponent<PlayerMovement> ().enabled = true;
		g.GetComponent<Attacks> ().enabled = true;
		g.GetComponent<SpriteRenderer> ().sprite = original;
		//  ^put stuff back the way it was

	}
}