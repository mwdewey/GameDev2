using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class init_spawner : MonoBehaviour {

	public List<GameObject> Players;

	// Use this for initialization
	void Start () {
		foreach(GameObject p in Players) {
			Instantiate(p, transform.position, Quaternion.identity);
		}
	}
}
