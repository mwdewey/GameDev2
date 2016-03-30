using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {

	public bool opened;
	float timer;

	// Use this for initialization
	void Start () {
		opened = false;
		timer = 30f;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "PlayerObject" && opened) {
			other.GetComponentsInParent<Score_Counter> ()[0].in_portal = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "PlayerObject" && opened) {
			other.GetComponentsInParent<Score_Counter> ()[0].in_portal = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (opened) {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				opened = false;
				GetComponent<SpriteRenderer> ().enabled = false;
				Debug.Log ("The portal has been closed");
			}
		}
	}
}
