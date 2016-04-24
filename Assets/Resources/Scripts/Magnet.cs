using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Magnet : Item {

	Transform holder_t;
	Transform t;
	public AudioClip coin_collect_sound;
	Score_Counter scorey;

	public override void Activate(){
		holder.GetComponent<PlayerController> ().held_item = null;
		holder.GetComponent<Score_Counter> ().magnetized = true;
		activated = true;
		collidy.enabled = true;
		collidy.radius *= 8f;
		holder_t = holder.GetComponent<Transform> ();
		t = GetComponent<Transform> ();
		scorey = holder.GetComponent<Score_Counter> ();
		t.position = holder_t.position;
		ParticleSystem.EmissionModule emitter = GetComponent<ParticleSystem> ().emission;
		emitter.enabled = true;
		rendy.enabled = true;
		StartCoroutine (KillMe ());
	}

	IEnumerator KillMe(){
		yield return new WaitForSeconds (20f);
		holder.GetComponent<Score_Counter> ().magnetized = false;
		Destroy (gameObject);
		activated = false;
	}

	void OnTriggerStay2D(Collider2D other){
		if (!activated) {
			return;
		}
		if (other.tag == "Coin" && !holder.GetComponent<PlayerController>().unconscious){
			other.GetComponent<Rigidbody2D>().velocity = Vector3.Scale((holder_t.position - other.transform.position).normalized, new Vector3(3f, 3f, 0));

			if (Vector3.Distance(holder_t.position, other.transform.position) <= 0.5 && other.GetComponent<Renderer>().enabled){
				scorey.score += 1;
				holder_t.Find ("Player 1 UI").transform.Find("Coin Text").gameObject.GetComponent<Text>().text = scorey.score.ToString();
				other.GetComponent<Renderer>().enabled = false;
				Destroy(other.gameObject);
				Manager.coins_remaining -= 1;
				holder.GetComponent<AudioSource> ().clip = coin_collect_sound;
				holder.GetComponent<AudioSource> ().Play ();
			}
		}
	}

	void Update(){
		if (activated) {
			t.position = holder_t.position;
			if (!scorey.magnetized) {
				scorey.magnetized = true;
			}
		}
	}
}
