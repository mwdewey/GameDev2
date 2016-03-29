using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {

	Transform t;
	public int health;

	// Use this for initialization
	void Start () {
		t = GetComponent<Transform> ();
		health = 20;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.D)) {
			t.position += new Vector3 (0.2f, 0.0f, 0.0f);
		}
		if (Input.GetKey (KeyCode.S)) {
			t.position += new Vector3 (0.0f, -0.2f, 0.0f);
		}
		if (Input.GetKey (KeyCode.A)) {
			t.position += new Vector3 (-0.2f, 0.0f, 0.0f);
		}
		if (Input.GetKey (KeyCode.W)) {
			t.position += new Vector3 (0.0f, 0.2f, 0.0f);
		}

	}
}
