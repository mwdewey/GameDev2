using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class N_PlayerController : NetworkBehaviour
{

    // Configurable options
    [HideInInspector]
    public string PID = "1"; // Joystick Number of a player
    public float MELEE_DAMAGE = 20;
    public float PROJECTILE_SPEED = 12;
    public float PLAYER_SPEED = 0.0f;
    public bool useKeyboard;

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

    // adding velocity since 2d rigidbody is not syncing
    private Vector2 velocity_local;
    private Vector2 prev_position_local;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
        velocity = rb.velocity;
        velocity_local = new Vector2(0, 0);
        prev_position_local = transform.position;

        // set PID to the count of players in scene
        PID = GameObject.FindGameObjectsWithTag("Player").Length.ToString();

        // if local player, set camera to focus
        if (isLocalPlayer) gameObject.AddComponent<N_CameraController>();

        //ignore all collisions between players
        List<GameObject> all_players = GameObject.FindGameObjectsWithTag("Player").ToList();
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
        switch (PID)
        {
            case "1": ring.color = new Color32(244, 67, 54, 255); break;
            case "2": ring.color = new Color32(33, 150, 243, 255); break;
            case "3": ring.color = new Color32(76, 175, 80, 255); break;
            case "4": ring.color = new Color32(255, 235, 59, 255); break;
        }

        anim = GetComponent<Animator>();
        anim.SetFloat("rangeSpeed", 3);
        anim.SetFloat("meleeSpeed", 3);
        playerKnockback = null;
        

    }

	void Update () {

        if (!isLocalPlayer) return;
		if (unconscious) return;

        
        // detect melee
        if(Input.GetButtonDown("Joy" + PID + "_MeleeAttack") || Input.GetKeyDown(KeyCode.J))
        {
            meleeAttack();
        }

        // detect range attack
        if (Input.GetButtonDown("Joy" + PID + "_RangedAttack") || Input.GetKeyDown(KeyCode.K))
        {
            rangeAttack();
        }

		if (Input.GetButtonDown ("Joy" + PID + "_UltimateAttack")) {
			GetComponent<Score_Counter> ().progress_portal ();
		}

        //debugger();
	}

    void FixedUpdate()
    {

        velocity_local.x = (transform.position.x - prev_position_local.x) / Time.fixedDeltaTime;
        velocity_local.y = (transform.position.y - prev_position_local.y) / Time.fixedDeltaTime;

        prev_position_local.Set(transform.position.x, transform.position.y);

        if (isLocalPlayer)
        {

            velocity.Set(0, 0);

            if (anim.GetInteger("PlayerState") == 1) return;

            if (!useKeyboard)
            {
                if (!unconscious)
                {
                    velocity.x = Input.GetAxis("Joy" + PID + "_LeftStickHorizontal") * PLAYER_SPEED;
                    velocity.y = Input.GetAxis("Joy" + PID + "_LeftStickVertical") * PLAYER_SPEED;
                }
            }
            else
            {
                if (!unconscious)
                {
                    velocity.x = Input.GetAxis("kb_horizontal") * PLAYER_SPEED;
                    velocity.y = Input.GetAxis("kb_vertical") * PLAYER_SPEED;
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

        }

        if (!isLocalPlayer) print(velocity.magnitude);

        // only change angle if moving
        if (velocity_local.magnitude > 0.5)
        {
            float angle = Mathf.Atan2(velocity_local.x, velocity_local.y) * Mathf.Rad2Deg;
            if (angle >= -45 && angle <= 45)   anim.SetInteger("DirectionState", 2); // up
            if (angle <= -135 || angle >= 135) anim.SetInteger("DirectionState", 3); // down
            if (angle < -45 && angle > -135)   anim.SetInteger("DirectionState", 0); // left
            if (angle > 45 && angle < 135)     anim.SetInteger("DirectionState", 1); // right
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

        // preforms melee attack network wide
        Cmd_MeleeAttack(direction,angle);

        // preform animation
        anim.SetTrigger("Melee");
    }

    [Command]
    void Cmd_MeleeAttack(Vector2 direction, Quaternion angle)
    {
        GameObject melee = (GameObject)Instantiate(melee_hitbox, new Vector3(transform.position.x + direction.x * (2f / 3), transform.position.y + direction.y * (2f / 3), 0), angle);
        melee.transform.parent = gameObject.transform;
        melee.GetComponent<CauseDamage>().my_parent_name = name;

        NetworkServer.Spawn(melee);

        Destroy(melee, 3.0f);
    }

    [Command]
    void Cmd_RangedAttack(GameObject ranged)
    {
        NetworkServer.Spawn(ranged);

        Destroy(ranged, 2.0f);
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
