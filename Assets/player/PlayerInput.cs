using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    Rigidbody2D rb;

    KeyCode up_key = KeyCode.W;
    KeyCode down_key = KeyCode.S;
    KeyCode left_key = KeyCode.S;
    KeyCode right_key = KeyCode.D;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(up_key)) rb.AddForce(new Vector2(0, 1));
        if (Input.GetKeyDown(down_key)) rb.AddForce(new Vector2(0, -1));
        if (Input.GetKeyDown(left_key)) rb.AddForce(new Vector2(-1,0));
        if (Input.GetKeyDown(right_key)) rb.AddForce(new Vector2(1,0));


	}
}
