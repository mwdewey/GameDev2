﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour {

    // Configurable options
    public string PID; // Joystick Number of a player
    public float MELEE_DAMAGE = 20;
    public float PROJECTILE_SPEED = 12;
    public float PLAYER_SPEED = 0.0f;
    public float POWER_RECHARGE_RATE = 10f;
    public bool useKeyboard;
    public Color32 playerColor = new Color32(244, 67, 54, 255);

	public CharCodes character;

    // Public required objects
    public GameObject melee_hitbox;
    public GameObject ranged_hitbox;
	public GameObject init_spawn_point;

    // Public non editor objects
    [HideInInspector]
    public bool unconscious;
	public bool locked;
	public float speed_boost;

    private bool awake = true;
    public Vector2 velocity;
    private Rigidbody2D rb;

    private Knockback playerKnockback;
    private SpriteRenderer ring;
    private Animator anim;
	private Rigidbody2D body;

	public Item held_item;
	public List<Item> item_list;
	public float health;
    public float power;
    private float POWER_MAX = 100;

	AudioSource Audio;
	public AudioClip melee_sound;
	public AudioClip ranged_sound;
	public AudioClip pickup_item_sound;

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

		body = GetComponent<Rigidbody2D> ();

        anim = GetComponent<Animator>();
        playerKnockback = null;
        
		held_item = null;
		item_list = new List<Item> ();

		locked = false;
		speed_boost = 1.0f;

		Audio = GetComponent<AudioSource> ();
    }

	public void Lock(bool which){
		if (which) {
			unconscious = true;
			locked = true;
            rb.isKinematic = true;
		} else {
			unconscious = false;
			locked = false;
            rb.isKinematic = false;
		}
	}

	void Update () {
		if (unconscious) return;

        // gain power of time
        power += Time.deltaTime * POWER_RECHARGE_RATE;
        if (power > POWER_MAX) power = POWER_MAX;

        // detect melee
		if(Input.GetButtonDown("Joy" + PID + "_MeleeAttack")  || Input.GetKeyDown(KeyCode.J))
        {
            meleeAttack();
			Audio.clip = melee_sound;
			Audio.Play ();
        }

        // detect range attack
		if ((Input.GetButtonDown("Joy" + PID + "_RangedAttack") && !Input.GetButton("Joy" + PID + "_MeleeAttack")) || Input.GetKeyDown(KeyCode.K))
        {
            rangeAttack();
			Audio.clip = ranged_sound;
			Audio.Play ();
        }

        // proto super
		if (Input.GetButtonDown("Joy" + PID + "_RangedAttack") && Input.GetButton("Joy" + PID + "_MeleeAttack"))
        {
            //rangeAttackUltra();
			//Audio.clip = ranged_sound;
			//Audio.Play ();
        }

		if (Input.GetButtonDown ("Joy" + PID + "_UltimateAttack")) {
			//ultimateAttack();
		}

		if (Input.GetButtonDown ("Joy" + PID + "_Item") || Input.GetKeyDown(KeyCode.L)) {
			GetComponent<Score_Counter> ().progress_portal ();
			if (held_item == null && item_list.Count > 0) {
                GetComponent<Animator>().SetTrigger("Pickup");
				held_item = item_list [0];
				item_list.Remove (held_item);
				held_item.Picked_Up ();
				held_item.holder = gameObject;
				GetComponent<AudioSource> ().clip = pickup_item_sound;
				GetComponent<AudioSource> ().Play ();
				//Debug.Log ("Picked up item: " + held_item.name);
			} 
			else if (held_item != null) {
				held_item.Activate ();

                // increment item used counter
                PlayerStats.getStats(PID).itemsUsed++;
			}
		}
		if (Input.GetButtonDown("Joy" + PID + "_Drop") && held_item != null) {
			held_item.Drop ();
			held_item = null;
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
				velocity.x = Input.GetAxis ("Joy" + PID + "_LeftStickHorizontal") * PLAYER_SPEED * speed_boost;
				velocity.y = Input.GetAxis ("Joy" + PID + "_LeftStickVertical") * PLAYER_SPEED * speed_boost;
			}
		}
        else
        {
			if (!unconscious) {
				velocity.x = Input.GetAxis ("kb_horizontal") * PLAYER_SPEED * speed_boost;
				velocity.y = Input.GetAxis ("kb_vertical") * PLAYER_SPEED * speed_boost;
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
        projectile.GetComponent<Rigidbody2D>().velocity = direction * PROJECTILE_SPEED + GetComponent<Rigidbody2D>().velocity;
        projectile.GetComponent<CauseKnockback>().my_parent_name = name; //tell it who made it

        // preform animation
        anim.SetTrigger("Range");

        // add attack to stats
        PlayerStats.getStats(PID).attacksDone++;
    }

    private void rangeAttackUltra()
    {
        float num_of_particles = 30;
        for (float i = 0; i < num_of_particles; i++)
        {
            float z_angle = i / num_of_particles * 360f;
            Vector2 direction = new Vector2(Mathf.Sin(Mathf.Deg2Rad * z_angle), Mathf.Cos(Mathf.Deg2Rad * z_angle));
            float correction = 360f * (num_of_particles - i) / num_of_particles;
			Quaternion angle = Quaternion.Euler(0, 0, correction);
            GameObject projectile = (GameObject)Instantiate(ranged_hitbox, transform.position + new Vector3(direction.x,direction.y,0)* 2, angle);
            projectile.transform.localEulerAngles = new Vector3(0, 0, correction);
            projectile.GetComponent<Rigidbody2D>().velocity = direction * PROJECTILE_SPEED + GetComponent<Rigidbody2D>().velocity;
            projectile.GetComponent<CauseKnockback>().my_parent_name = name;
        }


        // preform animation
        anim.SetTrigger("Range");

        // add attack to stats
        PlayerStats.getStats(PID).attacksDone++;
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

        // add attack to stats
        PlayerStats.getStats(PID).attacksDone++;
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
