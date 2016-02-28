using UnityEngine;
using System.Collections;

public class PlayerMovementTest : MonoBehaviour {

    public int PID;
    public float speed;

    private Rigidbody2D rb;
    private Vector2 velocity;

    private KeyCode up;
    private KeyCode down;
    private KeyCode left;
    private KeyCode right;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();
        velocity = new Vector2(0, 0);

        if (PID == 1)
        {
            up = KeyCode.W;
            down = KeyCode.S;
            left = KeyCode.A;
            right = KeyCode.D;
        }

        else if (PID == 2)
        {
            up = KeyCode.UpArrow;
            down = KeyCode.DownArrow;
            left = KeyCode.LeftArrow;
            right = KeyCode.RightArrow;
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
        velocity.x = 0;
        velocity.y = 0;

        if (Input.GetKey(up)) velocity.y += speed;
        if (Input.GetKey(down)) velocity.y -= speed;
        if (Input.GetKey(left)) velocity.x -= speed;
        if (Input.GetKey(right)) velocity.x += speed;

        rb.velocity = velocity;

	}
}
