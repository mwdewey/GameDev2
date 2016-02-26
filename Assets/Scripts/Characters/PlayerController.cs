using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour {

    public string PID; // Joystick Number of a player

	private bool awake = true;
    
    public float speed = 0.0f;
    public Vector2 velocity;
    private Rigidbody2D rb;
    int MAX_VELOCITY = 5;
    public bool useKeyboard;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = rb.velocity;

        //ignore all collisions between players
        List<GameObject> all_players = GameObject.FindGameObjectsWithTag("Player").ToList();
        for (int i = 0; i < all_players.Count; i++)
        {
            print(all_players[i].name);
            for (int j = 0; j < all_players.Count; j++)
            {
                if (all_players[i] != all_players[j])
                {
                    print("Canceling collisions between " + all_players[i] + " and " + all_players[j]);
                    Physics2D.IgnoreCollision(all_players[i].GetComponent<Collider2D>(), all_players[j].GetComponent<Collider2D>());
                }
            }
        }

    }

	void Update () {
        debugger();
	}

    void FixedUpdate()
    {
        if (!useKeyboard)
        {
            velocity.x = Input.GetAxis(PID + "_LeftStickHorizontal") * speed;
            velocity.y = Input.GetAxis(PID + "_LeftStickVertical") * speed;
        }
        else
        {
            velocity.x = Input.GetAxis("kb_horizontal") * speed;
            velocity.y = Input.GetAxis("kb_vertical") * speed;
        }

        rb.velocity = velocity;
    }

	bool isAwake() {
		return awake;
	}

	bool sleep() {
		awake = false;
	}

	bool wake() {
		awake = true;
	}

    void debugger()
    {
        if (Input.GetButtonDown("Joy1_MeleeAttack"))
        {
            print("melee attack");
        }
        if (Input.GetButtonDown("Joy1_RangedAttack"))
        {
            print("ranged attack");
        }
        if (Input.GetButtonDown("Joy1_SpecialAttack"))
        {
            print("special attack");
        }
        if (Input.GetButtonDown("Joy1_UltimateAttack"))
        {
            print("ultimate attack");
        }
    }

}
