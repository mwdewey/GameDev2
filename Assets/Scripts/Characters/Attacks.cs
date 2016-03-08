using UnityEngine;
using System.Collections;

public class Attacks : MonoBehaviour {

	int MELEE_DAMAGE = 20;
	int PROJECTILE_SPEED = 12;
	Vector2 direction;
	public GameObject melee_hitbox;
	public GameObject ranged_hitbox;
	GameObject my_weapon;
	public bool unconscious; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (unconscious) return;

		//find the newest direction that the player is facing, so long as they're moving
		if (!(GetComponent<Rigidbody2D> ().velocity.x == 0 && GetComponent<Rigidbody2D> ().velocity.y == 0)) {
			Vector2 old_direction = direction;//in case direction is 0,0 after normalizaiton anyway
			direction = GetComponent<Rigidbody2D> ().velocity;
			direction.Normalize ();
			if (direction.x==0 && direction.y==0)
				direction = old_direction;
		}

		//now melee attack if J is pressed
		if (Input.GetKeyDown (KeyCode.J) && my_weapon == null) {
			my_weapon = (GameObject)Instantiate (melee_hitbox, new Vector3 (transform.position.x + direction.x*(2f/3), transform.position.y + direction.y*(2f/3), 0), Quaternion.identity);
			my_weapon.transform.parent = gameObject.transform; //parenting lets the melee object move with the parent
		}

		//ranged attack if K is pressed
		if (Input.GetKeyDown (KeyCode.K) ) {
			GameObject projectile = (GameObject)Instantiate (ranged_hitbox, new Vector3 (transform.position.x + direction.x*(2f/3), transform.position.y + direction.y*(2f/3), 0), Quaternion.identity);
			projectile.GetComponent<Rigidbody2D>().velocity = direction*PROJECTILE_SPEED; 
			projectile.GetComponent<CauseKnockback> ().my_parent_name = name; //tell it who made it
			//print("direction:"+direction);
			/*
			if (projectile.GetComponent<ObjectLifetime> ().dieAfterFrames) {
				projectile.GetComponent<ObjectLifetime> ().die_after_frames = 20;
			} 
			else {
				projectile.GetComponent<ObjectLifetime> ().die_after_millis = 20;
			}
			*/
		}


	}
}
