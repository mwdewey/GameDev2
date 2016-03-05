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

    private Animator anim;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();

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

        // only handling cases with dual input
        if (velocity.x != 0 && velocity.y != 0)
        {
            float angle = Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;

            // up
            if (angle >= -45 && angle <= 45)
            {
                anim.SetFloat("delta_x", 0);
                anim.SetFloat("delta_y", 1);
            }

            // down
            else if (angle <= -135 || angle >= 135)
            {
                anim.SetFloat("delta_x", 0);
                anim.SetFloat("delta_y", -1);
            }

            // left
            if (angle < -45 && angle > -135)
            {
                anim.SetFloat("delta_x", -1);
                anim.SetFloat("delta_y", 0);
            }

            // right
            if (angle > 45 && angle < 135)
            {
                anim.SetFloat("delta_x", 1);
                anim.SetFloat("delta_y", 0);
            }

            print(angle);
        }

        else
        {
            anim.SetFloat("delta_x", velocity.x);
            anim.SetFloat("delta_y", velocity.y);
        }


	}
}
