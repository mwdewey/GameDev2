using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed = 10;
    private Vector2 velocity;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = rb.velocity;
    }

	void Update () {
		
	}

    void FixedUpdate()
    {
        velocity.x = Input.GetAxis("Horizontal") * speed;
        velocity.y = Input.GetAxis("Vertical") * speed;
        
        rb.velocity = velocity;
    }



}
