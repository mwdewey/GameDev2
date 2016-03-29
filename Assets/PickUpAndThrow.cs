using UnityEngine;
using System.Collections;

public class PickUpAndThrow : MonoBehaviour {

	GameObject heldPlayer=null; 
	GameObject launched = null;
	int LAUNCH_SPEED = 15;
	float FLIGHT_TIME = 2f;
	public GameObject melee_hitbox;
	private Vector2 direction = new Vector2 (0, 0);
	private string dir_string="";

	// Use this for initialization
	void Start () {
	
	}
		

	// Update is called once per frame
	void Update () {
		if (GetComponent<PlayerController> ().unconscious) {
			//if I get knocked out I have to drop anyone I've picked up
			if (heldPlayer != null) {
				Physics2D.IgnoreCollision(GetComponentInChildren<PolygonCollider2D>(), heldPlayer.GetComponentInChildren<PolygonCollider2D>(), false);
				heldPlayer = null;
			}
			return; //the only thing we want to do if we're unconscious is drop anyone we
					//are currently holding
		}

		dir_string = getDirection (ref direction);

		if ((Input.GetKeyDown (KeyCode.L) || Input.GetKeyDown(KeyCode.JoystickButton2)) && heldPlayer==null) {
			//pick up an unconscious player
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll (transform.position, 1);
			foreach (Collider2D c in hitColliders) {
				if (c.gameObject.tag=="PlayerObject" && c.gameObject.name != name && c.gameObject.GetComponent<PlayerController> ().unconscious) {
					heldPlayer = c.gameObject;
					c.gameObject.GetComponent<SpriteRenderer> ().sortingOrder = 3;
					Physics2D.IgnoreCollision(GetComponentInChildren<PolygonCollider2D>(), c.gameObject.GetComponentInChildren<PolygonCollider2D>(), true);
					print ("picked up " + c.gameObject.name);
					break;
				}
			}
			Destroy (heldPlayer.transform.Find ("THIS IS THE FLYING GUY'S HITBOX").gameObject);
		}
        else if ((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.JoystickButton2)) && heldPlayer != null)
        {
			//throw the captured player
			print ("tossed " + heldPlayer.name);
			launched = heldPlayer;
			heldPlayer.GetComponent<SpriteRenderer> ().sortingOrder = 0;
			//enable Collision again
			Physics2D.IgnoreCollision(GetComponentInChildren<PolygonCollider2D>(), heldPlayer.GetComponentInChildren<PolygonCollider2D>(), false);
			heldPlayer = null;
		}
		if (heldPlayer != null && heldPlayer.GetComponent<PlayerController>().unconscious==false){
			//my victim is awake!
			Physics2D.IgnoreCollision(GetComponentInChildren<PolygonCollider2D>(), heldPlayer.GetComponentInChildren<PolygonCollider2D>(), false);
			heldPlayer.GetComponent<PlayerController> ().setKnockBack(null);
			heldPlayer = null;
		}
		if (launched!=null) {
			// get direction
			int directionState = GetComponent<Animator>().GetInteger("DirectionState");
			switch (dir_string)
			{
			case "Left": break; // Left
			case "Right": break; // Right
			case "Up": launched.transform.position = new Vector2(launched.transform.position.x, launched.transform.position.y + .5f); break; // Up
			case "Down": launched.transform.position = new Vector2(launched.transform.position.x, launched.transform.position.y - .5f); break; // Down
			}
				
			print ("launched!");
			launched.GetComponent<PlayerController> ().setKnockBack (new Knockback (FLIGHT_TIME, direction * LAUNCH_SPEED));

			//give the launched player a damaging hitbox
			GameObject launchedHitbox = (GameObject)Instantiate(melee_hitbox, launched.transform.position, Quaternion.identity);
			launchedHitbox.transform.parent = launched.transform;
			launchedHitbox.name = "THIS IS THE FLYING GUY'S HITBOX";
			launchedHitbox.GetComponent<CauseDamage> ().DAMAGE = 40;
			launchedHitbox.GetComponent<CauseDamage> ().dontdamage.Add (name);
			launchedHitbox.GetComponent<CauseDamage> ().dontdamage.Add (launched.name);
			launchedHitbox.GetComponent<CauseDamage> ().launched_player = true;
			launchedHitbox.GetComponent<ObjectLifetime> ().dieAfterFrames = false;
			launchedHitbox.GetComponent<ObjectLifetime> ().die_after_seconds = FLIGHT_TIME;
			//^this takes care of Destroy (launchedHitbox, FLIGHT_TIME);
			launched = null;
		}
	}

	string getDirection(ref Vector2 direction){
		int directionState = GetComponent<Animator>().GetInteger("DirectionState");
		//Quaternion angle = Quaternion.Euler(0, 0, 0);
		switch (directionState)
		{
		case 0: direction.Set(-1, 0); return "Left"; // Left
		case 1: direction.Set(1, 0); return "Right"; // Right
		case 2: direction.Set(0, 1); return "Up"; // Up
		case 3: direction.Set(0, -1); return "Down"; // Down
		}
		return "";
	}

	void FixedUpdate(){
		if (heldPlayer != null) {
			//keep the captured player with me
			print ("holding " + heldPlayer.name);
			float x_offset = 0;
			float y_offset = 0;

			SpriteRenderer hp_sprite= heldPlayer.GetComponent<SpriteRenderer>();
			switch (dir_string) {
			case "Left":
				hp_sprite.sortingOrder = 3;
				x_offset = -.2f;
				y_offset = .3f;
				break;
			case "Right":
				hp_sprite.sortingOrder = 3;
				x_offset = .2f;
				y_offset = .3f;
				break;
			case "Up":
				hp_sprite.sortingOrder = -1;
				x_offset = 0;
				y_offset = .4f; 
				break;
			case "Down":
				hp_sprite.sortingOrder = 3;
				x_offset = 0;
				y_offset = .3f;
				break;
			}
			heldPlayer.transform.position = new Vector2(gameObject.transform.position.x + x_offset, gameObject.transform.position.y + y_offset);
		}
	}
}
