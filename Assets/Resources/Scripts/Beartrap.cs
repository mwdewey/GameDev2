using UnityEngine;
using System.Collections;

public class Beartrap : Item {

	bool primed;
    bool sprung;
	public Sprite sprung_sprite;
	public AudioClip clank_sound;

	public override void Activate(){
        sprung = false;
		holder.GetComponent<PlayerController> ().held_item = null;
		activated = true;
		transform.position = holder.transform.position;
		collidy.radius = .29f;
        collidy.offset = new Vector2(0, -0.1f);
		collidy.enabled = true;
		primed = false;
		rendy.enabled = true;
		StartCoroutine (Prime ());
	}

	void OnTriggerEnter2D(Collider2D other){
		if (activated && !sprung) {
			if (primed && other.tag == "PlayerObject") {
				other.transform.position = transform.position + new Vector3(0f, 1f, 0f);
				other.GetComponent<PlayerController> ().Lock (true);
				StartCoroutine (ReleaseVictim (other.GetComponent<PlayerController> ()));
                sprung = true;
				GetComponent<SpriteRenderer> ().sprite = sprung_sprite;
				GetComponent<AudioSource> ().clip = clank_sound;
				GetComponent<AudioSource> ().Play ();
			} 
			else {
				return;
			}
		}
		else if (!activated && other.tag == "PlayerObject") {
			other.GetComponent<PlayerController> ().item_list.Add (this);
			ring.enabled = true;
		}
	}

	IEnumerator Prime(){
		yield return new WaitForSeconds (2f);
		primed = true;
	}

	IEnumerator ReleaseVictim(PlayerController victim){
		yield return new WaitForSeconds (5f);
		victim.Lock (false);
		Destroy (gameObject);
	}
}
