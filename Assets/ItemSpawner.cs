using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour {

	public List<GameObject> items;
	GameObject current;
	Item current_component;
	public float respawn_time;
	float timer;

	// Use this for initialization
	void Start () {
		SpawnItem ();
	}

	void SpawnItem(){
		current = (GameObject)Instantiate (items [Mathf.FloorToInt (Random.Range (0, items.Count - 0.0000001f))], transform.position, Quaternion.identity);
		current_component = current.GetComponent<Item> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (current == null) {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				SpawnItem ();
			}
		}
		else if (current_component.holder != null) {
			current = null;
			current_component = null;
			timer = respawn_time;
		}
	}
}
