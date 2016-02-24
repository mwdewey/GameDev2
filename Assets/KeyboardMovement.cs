using UnityEngine;
using System.Collections;

public class KeyboardMovement : MonoBehaviour {
       
    public Vector2 velocity;
    private Rigidbody2D rb;
    public float speed = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = rb.velocity;
    }

    void FixedUpdate()
    {
        velocity.x = Input.GetAxis("kb_horizontal") * speed;
        velocity.y = Input.GetAxis("kb_vertical") * speed;

        rb.velocity = velocity;
    }
}
