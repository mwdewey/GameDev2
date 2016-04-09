using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {

	float life;
	float damager;
	List<GameObject> victims;
	public AudioClip get_burned_sound;
	public AudioClip its_lit_sound;

	// Use this for initialization
	void Start () {
		GetComponent<AudioSource> ().clip = its_lit_sound;
		GetComponent<AudioSource> ().Play ();
		life = 7f;
		damager = 0.5f;
		victims = new List<GameObject> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Ring") {
			victims.Add (other.transform.parent.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Ring") {
			victims.Remove (other.transform.parent.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		life -= Time.deltaTime;
		damager -= Time.deltaTime;
		if (life <= 0) {
			Destroy (gameObject);
		} 
		else if (damager <= 0) {
			damager = 0.5f;
			foreach (GameObject victim in victims) {
				victim.GetComponent<Consciousness> ().TakeDamage (5);
				GetComponent<AudioSource> ().clip = get_burned_sound;
				GetComponent<AudioSource> ().Play ();
			}
		}
	}
}
