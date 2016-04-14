using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarpBlock : Item {

	public AudioClip warp_sound;

	public override void Activate () {
		List<GameObject> players = new List<GameObject> (GameObject.FindGameObjectsWithTag ("PlayerObject"));
		players.Remove (holder);
		if (players.Count == 0){
			Destroy (gameObject);
			return;
		}
		holder.GetComponent<AudioSource> ().clip = warp_sound;
		holder.GetComponent<AudioSource> ().Play ();
		int choice = (int) Mathf.Floor(Random.Range (0, players.Count - 0.0001f));
		holder.GetComponent<Transform> ().position = players [choice].transform.position;
		holder.GetComponent<PlayerController> ().held_item = null;
		Destroy (gameObject);
	}
}
