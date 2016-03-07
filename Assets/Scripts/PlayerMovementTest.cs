using UnityEngine;
using System.Collections;

public class PlayerMovementTest : MonoBehaviour {

    public int PID;
    public float speed;
	private SpriteRenderer ring;

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

        ring = transform.Find("Ring").gameObject.GetComponent<SpriteRenderer>();

		if (PID == 1) {
			ring.color = Color.red;
		}
		else if (PID == 2) {
			ring.color = Color.blue;
		}
		else if (PID == 3) {
			ring.color = Color.green;
		}
		else if (PID == 4) {
			ring.color = Color.yellow;
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

        if (PID == 1)
        {
            if (Input.GetKeyDown(KeyCode.J)) anim.SetTrigger("Melee");
            if (Input.GetKeyDown(KeyCode.K)) anim.SetTrigger("Range");
            if (Input.GetKeyDown(KeyCode.L)) anim.SetTrigger("Flinch");
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
}
