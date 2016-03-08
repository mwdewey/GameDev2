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

    private Knockback playerKnockback;
    private SpriteRenderer ring;
    private Animator anim;

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

        // Color ring around players
        ring = transform.Find("Ring").gameObject.GetComponent<SpriteRenderer>();
        switch (PID)
        {
            case "1": ring.color = Color.red; break;
            case "2": ring.color = Color.red; break;
            case "3": ring.color = Color.red; break;
            case "4": ring.color = Color.red; break;
        }

        anim = GetComponent<Animator>();
        playerKnockback = null;
        

    }

	void Update () {
        debugger();
	}

    void FixedUpdate()
    {
        if (!useKeyboard)
        {
            velocity.x = Input.GetAxis("Joy" + PID + "_LeftStickHorizontal") * speed;
            velocity.y = Input.GetAxis("Joy" + PID + "_LeftStickVertical") * speed;
        }
        else
        {
            velocity.x = Input.GetAxis("kb_horizontal") * speed;
            velocity.y = Input.GetAxis("kb_vertical") * speed;
        }

        rb.velocity = velocity;

        // only change angle if moving
        if (velocity.magnitude > 0.1)
        {
            float angle = Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;
            if (angle >= -45 && angle <= 45) anim.SetInteger("DirectionState", 2); // up
            if (angle <= -135 || angle >= 135) anim.SetInteger("DirectionState", 3); // down
            if (angle < -45 && angle > -135) anim.SetInteger("DirectionState", 0); // left
            if (angle > 45 && angle < 135) anim.SetInteger("DirectionState", 1); // right
        }
    }

	bool isAwake() {
		return awake;
	}

	bool sleep() {
		awake = false;
        return true;
	}

	bool wake() {
		awake = true;
        return true;
	}

    public void setKnockBack(Knockback knockback)
    {
        this.playerKnockback = knockback;
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
