using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour {

	public GameObject coin;
	GameObject current;
	bool exists;
	public float respawn_time;
	float timer;

	// Use this for initialization
	void Start () {
		SpawnCoin ();
	}

	void SpawnCoin(){
		current = (GameObject)Instantiate (coin, transform.position, Quaternion.identity);
		exists = true;
	}

	// Update is called once per frame
	void Update () {
		if (current == null) {
			if (exists) {
				exists = false;
				timer = respawn_time;
			} 
			else {
				timer -= Time.deltaTime;
				if (timer <= 0) {
					SpawnCoin ();
				}
			}
		}
	}
}
