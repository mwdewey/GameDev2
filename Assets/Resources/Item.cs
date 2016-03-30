using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour {

	public SpriteRenderer ring;
	public SpriteRenderer rendy;
	public CircleCollider2D collidy;
	public GameObject holder;
	public bool activated;

	// Use this for initialization
	void Start () {
		ring = GetComponentsInChildren<SpriteRenderer> () [1];
		rendy = GetComponent<SpriteRenderer> ();
		collidy = GetComponent<CircleCollider2D> ();
		activated = false;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "PlayerObject") {
			other.GetComponent<PlayerController> ().item_list.Add (this);
			ring.enabled = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "PlayerObject") {
			other.GetComponent<PlayerController> ().item_list.Remove (this);
			ring.enabled = false;
		}
	}

	public void Picked_Up(){
		ring.enabled = false;
		rendy.enabled = false;
		collidy.enabled = false;
	}

	public virtual void Activate(){
		activated = true;
	}

	public void Drop(){
		Debug.Log ("YOU HAVE DROPPED OUT A BLUE SHELL");
		rendy.enabled = true;
		collidy.enabled = true;
		transform.position = holder.transform.position - new Vector3 (0.0f, 0.5f, 0.0f);
		holder = null;
	}
}
