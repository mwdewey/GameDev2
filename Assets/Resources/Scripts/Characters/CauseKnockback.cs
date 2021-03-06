﻿using UnityEngine;
using System.Collections;

public class CauseKnockback : MonoBehaviour {

	float KNOCKBACK_AMOUNT = 10; //variable constant, change in testing
    private readonly float KNOCKBACK_TIME = .1f;
	public string my_parent_name = ""; //this is what the object stores their parent's name in
	public bool die_on_contact = true;
    public GameObject explosion;
    private string pid;

	// Use this for initialization
	void Start () {
		if (transform.parent != null) {
            pid = transform.parent.GetComponent<PlayerController>().PID;
			my_parent_name = transform.parent.name;
		}//if the object is a projectile, it won't have a parent
		//so, when a projectile is made, it will have this variable 
		//assigned for it in the Attacks script.

	}

	void OnTriggerEnter2D(Collider2D c){
		//print ("Pokeball pushes back "+c.gameObject.name);
		if (c.gameObject.GetComponent<ReceiveKnockback>()!=null && c.gameObject.name != my_parent_name && !c.GetComponent<PlayerController>().locked) {
            c.gameObject.GetComponent<Consciousness>().TakeDamage(3, pid);
            //if what we hit is a player and isn't the player who made us...
			Vector2 knockback = GetComponent<Rigidbody2D>().velocity;
			knockback.Normalize ();
			knockback.x *= KNOCKBACK_AMOUNT;
			knockback.y *= KNOCKBACK_AMOUNT;
            Knockback knockbackObject = new Knockback(KNOCKBACK_TIME,knockback);
            c.gameObject.SendMessage("GetKnockedBack", knockbackObject);
			//...activate the GetKnockedBack function of the thing we just hit
			//causing them to fly backward! In players, this function is in PlayerMovement.
		}
		//ADD EXCEPTIONS FOR DIE_ON_CONTACT object destroys here \/
		if (die_on_contact && c.gameObject.name != my_parent_name && c.gameObject.name!="Ring" 
			&& c.tag != "Coin" && c.tag != "Item" && c.tag != "Melee Hitbox" && c.tag != "Portal"
			&& c.tag != "Death Floor"){
			//if we hit ANYTHING but the player we came from
            GameObject expl_temp = (GameObject) Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(expl_temp, expl_temp.GetComponent<ParticleSystem>().startLifetime);
			Destroy (gameObject);
		}
	}
}
