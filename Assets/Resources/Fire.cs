using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {

	float life;
	float damager;
	List<GameObject> victims;
	public AudioClip get_burned_sound;
	public AudioClip its_lit_sound;
	public Sprite sprite1;
	public Sprite sprite2;
	SpriteRenderer rendy;

    public GameObject source;

	// Use this for initialization
	void Start () {
		GetComponent<AudioSource> ().clip = its_lit_sound;
		GetComponent<AudioSource> ().Play ();
		life = 7f;
		damager = 0.5f;
		victims = new List<GameObject> ();
		rendy = GetComponent<SpriteRenderer> ();
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

	void SwitchSprite(){
		if (rendy.sprite == sprite1) {
			rendy.sprite = sprite2;
		} 
		else {
			rendy.sprite = sprite1;
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
			SwitchSprite ();
			damager = 0.5f;
			foreach (GameObject victim in victims) {
                victim.GetComponent<Consciousness>().TakeDamage(20, source.GetComponent<PlayerController>().PID);
				GetComponent<AudioSource> ().clip = get_burned_sound;
				GetComponent<AudioSource> ().Play ();

                // update damage stats
                PlayerStats.getStats(source.GetComponent<PlayerController>().PID).damageDone += 20;
                PlayerStats.getStats(victim.gameObject.GetComponent<PlayerController>().PID).damageReceived += 20;

			}
		}
	}
}
