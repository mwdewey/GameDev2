using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerMovement : MonoBehaviour {

	public float speed = 10;
    public Vector2 velocity;
    private Rigidbody2D rb;
	int MAX_VELOCITY = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = rb.velocity;

		//ignore all collisions between players
		List<GameObject> all_players = GameObject.FindGameObjectsWithTag("Player").ToList();
		for (int i = 0; i < all_players.Count; i++) {
			print(all_players[i].name);
			for (int j = 0; j < all_players.Count; j++) {
				if (all_players [i] != all_players [j]) {
					print("Canceling collisions between "+all_players[i]+" and "+all_players[j]);
					Physics2D.IgnoreCollision (all_players [i].GetComponent<Collider2D> (), all_players [j].GetComponent<Collider2D> ());
				}
			}
		}

	}

	void Update () {
		
	}

    void FixedUpdate()
    {

		velocity.x = Input.GetAxis ("Horizontal") * speed;
		velocity.y = Input.GetAxis ("Vertical") * speed;

        //velocity.x = Input.GetAxis("kb_horizontal") * speed;
        //velocity.y = Input.GetAxis("kb_vertical") * speed;

        
        rb.velocity = velocity;
			
	}


	//Looking for GetKnockedBack? That's in ReceiveKnockback

}
