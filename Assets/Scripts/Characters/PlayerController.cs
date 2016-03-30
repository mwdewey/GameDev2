using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour {

    // Configurable options
    public string PID; // Joystick Number of a player
    public float MELEE_DAMAGE = 20;
    public float PROJECTILE_SPEED = 12;
    public float PLAYER_SPEED = 0.0f;
    public bool useKeyboard;
    public Color32 playerColor = new Color32(244, 67, 54, 255);

	public string character;

    // Public required objects
    public GameObject melee_hitbox;
    public GameObject ranged_hitbox;
	public GameObject init_spawn_point;

    // Public non editor objects
    [HideInInspector]
    public bool unconscious;

    private bool awake = true;
    private Vector2 velocity;
    private Rigidbody2D rb;

    private Knockback playerKnockback;
    private SpriteRenderer ring;
    private Animator anim;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
        velocity = rb.velocity;

        //ignore all collisions between players
        List<GameObject> all_players = GameObject.FindGameObjectsWithTag("PlayerObject").ToList();
        for (int i = 0; i < all_players.Count; i++)
        {
            for (int j = 0; j < all_players.Count; j++)
            {
                if (all_players[i] != all_players[j])
                {
                    Physics2D.IgnoreCollision(all_players[i].GetComponent<Collider2D>(), all_players[j].GetComponent<Collider2D>());
                }
            }
        }

        // Color ring around players
        ring = transform.Find("Ring").gameObject.GetComponent<SpriteRenderer>();
        ring.color = playerColor;

        anim = GetComponent<Animator>();
        anim.SetFloat("rangeSpeed", 3);
        anim.SetFloat("meleeSpeed", 3);
        playerKnockback = null;
        

    }

	void Update () {
		if (unconscious) return;

        // detect melee
        if(Input.GetButtonDown("Joy" + PID + "_MeleeAttack"))
        {
            meleeAttack();
        }

        // detect range attack
        if (Input.GetButtonDown("Joy" + PID + "_RangedAttack"))
        {
            rangeAttack();
        }

		if (Input.GetButtonDown ("Joy" + PID + "_UltimateAttack")) {
			GetComponent<Score_Counter> ().progress_portal ();
			ultimateAttack();
		}

        //if (PID.Equals("1")) print(rb.velocity);


        //debugger();
	}

    void FixedUpdate()
    {
		velocity.Set (0, 0);

		if (anim.GetInteger("PlayerState")==1) return;

		if (!useKeyboard) {
			if (!unconscious) {
				velocity.x = Input.GetAxis ("Joy" + PID + "_LeftStickHorizontal") * PLAYER_SPEED;
				velocity.y = Input.GetAxis ("Joy" + PID + "_LeftStickVertical") * PLAYER_SPEED;
			}
		}
        else
        {
			if (!unconscious) {
				velocity.x = Input.GetAxis ("kb_horizontal") * PLAYER_SPEED;
				velocity.y = Input.GetAxis ("kb_vertical") * PLAYER_SPEED;
			}
        }

        // add knockback velocity
        if (playerKnockback != null)
        {
            playerKnockback.timeRemaining -= Time.fixedDeltaTime;
            if (playerKnockback.timeRemaining < 0) playerKnockback.isActing = false;
            if (playerKnockback.isActing) velocity += playerKnockback.force;
        }

        rb.velocity = velocity;

        // only change angle if moving
        if (velocity.magnitude > 0.5)
        {
            float angle = Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;
            if (angle >= -45 && angle <= 45)   anim.SetInteger("DirectionState", 2); // up
            if (angle <= -135 || angle >= 135) anim.SetInteger("DirectionState", 3); // down
            if (angle < -45 && angle > -135)   anim.SetInteger("DirectionState", 0); // left
            if (angle > 45 && angle < 135)     anim.SetInteger("DirectionState", 1); // right

            // if not moving and anim is walking, make anim idle
            if (anim.GetInteger("PlayerState") == 2) anim.SetInteger("PlayerState", 3);
        }

        // if moving and anim is idle, make anim moving
        else if (anim.GetInteger("PlayerState") == 3) anim.SetInteger("PlayerState", 2);

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
        playerKnockback = knockback;
		if (anim.GetInteger ("PlayerState") == 1) playerKnockback=null;
    }

    private void rangeAttack()
    {
        // get position to fire range attack
        int directionState = anim.GetInteger("DirectionState");
        Vector2 direction = new Vector2(0,0);
        Quaternion angle = Quaternion.Euler(0,0,0);

        switch (directionState)
        {
            case 0: direction.Set(-1, 0); angle = Quaternion.Euler(0, 0, 90); break; // Left
            case 1: direction.Set(1, 0); angle = Quaternion.Euler(0, 0, -90);  break; // Right
            case 2: direction.Set(0, 1); angle = Quaternion.Euler(0, 0, 0);  break; // Up
            case 3: direction.Set(0, -1); angle = Quaternion.Euler(0, 0, 180);  break; // Down
        }

        GameObject projectile = (GameObject)Instantiate(ranged_hitbox, new Vector3(transform.position.x + direction.x * (2f / 3), transform.position.y + direction.y * (2f / 3), 0), angle);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * PROJECTILE_SPEED;
        projectile.GetComponent<CauseKnockback>().my_parent_name = name; //tell it who made it

        // preform animation
        anim.SetTrigger("Range");
    }

    private void meleeAttack()
    {
        // get position to fire range attack
        int directionState = anim.GetInteger("DirectionState");
        Vector2 direction = new Vector2(0, 0);
        Quaternion angle = Quaternion.Euler(0, 0, 0);

        switch (directionState)
        {
            case 0: direction.Set(-1, 0); break; // Left
            case 1: direction.Set(1, 0); break; // Right
            case 2: direction.Set(0, 1); angle = Quaternion.Euler(0, 0, 90);  break; // Up
            case 3: direction.Set(0, -1); angle = Quaternion.Euler(0, 0, 90); break; // Down
        }

        GameObject melee = (GameObject) Instantiate(melee_hitbox, new Vector3(transform.position.x + direction.x * (2f / 3), transform.position.y + direction.y * (2f / 3), 0), angle);
        melee.transform.parent = gameObject.transform;

        // preform animation
        anim.SetTrigger("Melee");
    }

	private void ultimateAttack()
	{
		gameObject.GetComponent<ultimateAttackController>().handleUltimateInput();
		//	TODO: hook in ultimate animation
		//int directionState = anim.GetInteger("DirectionState");
		// perform animation
		//anim.SetTrigger("Melee");
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
