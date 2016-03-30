using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class movement : MonoBehaviour {

	Transform t;
	public bool trapped;
	int health;
	public List<Item> item_list;
	Item held_item;
	public int pid;
	public int score;
	public bool dummy;

	// Use this for initialization
	void Start () {
		t = GetComponent<Transform> ();
		health = 20;
		held_item = null;
		item_list = new List<Item> ();
		//score = 0;
	}

	public void TakeDamage(int amount){
		health -= amount;
	}
	
	// Update is called once per frame
	void Update () {
		if (trapped || dummy) {
			return;
		}
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
		if (Input.GetKeyDown (KeyCode.L)) {
			if (held_item == null && item_list.Count > 0) {
				held_item = item_list [0];
				item_list.Remove (held_item);
				held_item.Picked_Up ();
				held_item.holder = gameObject;
				Debug.Log ("Picked up item: " + held_item.name);
			} 
			else if (held_item != null){
				held_item.Activate ();
				held_item = null;
			}
		}
		if (Input.GetKeyDown (KeyCode.P) && held_item != null) {
			held_item.Drop ();
			held_item = null;
		}
	}
}
