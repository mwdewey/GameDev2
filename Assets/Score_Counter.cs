using UnityEngine;
using System.Collections;

public class Score_Counter : MonoBehaviour {

	int score;
	Transform t;

	// Use this for initialization
	void Start () {
		score = 0;
		t = GetComponent<Transform> ();
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Coin") {
			other.GetComponent<Rigidbody2D> ().velocity = Vector3.Scale((t.position - other.transform.position).normalized, new Vector3(1.2f, 1.2f, 0));
		}
		if (Vector3.Distance(t.position, other.transform.position) <= 0.5) {
			score += 1;
			other.GetComponent<Renderer>().enabled = false;
			Destroy (other);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Coin") {
			other.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			Debug.Log ("Your score: " + score);
		}
	}
}
