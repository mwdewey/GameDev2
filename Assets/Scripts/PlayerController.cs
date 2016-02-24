using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private string PID; // Joystick Number of a player

	public float speed = 0.0f;
    private Vector2 velocity;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = rb.velocity;
    }

	void Update () {
        debugger();
	}

    void FixedUpdate()
    {
        velocity.x = Input.GetAxis("Joy1_LeftStickHorizontal") * speed;
        velocity.y = Input.GetAxis("Joy1_LeftStickVertical") * speed; 
        
        rb.velocity = velocity;
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
