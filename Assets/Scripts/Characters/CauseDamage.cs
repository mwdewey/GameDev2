using UnityEngine;
using System.Collections;

public class CauseDamage : MonoBehaviour {
	/* CauseDamage is a script which performs the most unexpected function
	 * of handling the dealing of damage. This script belongs on anything that can
	 * cause contact damage to a player, causing them to lost consciousness. 
	 * IMPORTANT:
	 * The object this is assigned to must have both a Collider2D and a Rigidbody2D. 
	 * The rigidbody must have Is Kinematic set on, and the collider must is trigger
	 * set on as well.
	*/

	int DAMAGE = 10; //variable constant, change in testing
	public string my_parent_name = ""; //this is what the object stores their parent's name in
	public bool die_on_contact = false;

	// Use this for initialization
	void Start () {
		if (transform.parent != null) {
			my_parent_name = transform.parent.name;
		}//if the object is a projectile, it won't have a parent
		//so, when a projectile is made, it will have this variable 
		//assigned for it in the Attacks script.

	}
	
	// Update is called once per frame
	void Update () {
		//lol
	}

	void OnTriggerEnter2D(Collider2D c){
		//print ("I hit "+c.gameObject.name);
		if (c.gameObject.tag == "Player" && c.gameObject.name != my_parent_name) {
			//if what we hit is a player and isn't the player who made us...
			c.gameObject.SendMessage ("TakeDamage", DAMAGE);
			//...activate the TakeDamage function of the thing we just hit
			//causing them to take DAMAGE amount of damage
		}
		if (die_on_contact && c.gameObject.name != my_parent_name){
			//if we hit ANYTHING but the player we came from
			Destroy (gameObject);
		}
	}
}
