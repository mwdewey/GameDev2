﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour {

    private string PID; // Joystick Number of a player
    
    public float speed = 0.0f;
    public Vector2 velocity;
    private Rigidbody2D rb;
    int MAX_VELOCITY = 5;

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
