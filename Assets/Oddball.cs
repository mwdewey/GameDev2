using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Oddball : Item {

	public float score_timer;
	float timer;

	// Use this for initialization
	void Start () {
		ring = GetComponentsInChildren<SpriteRenderer> () [1];
		rendy = GetComponent<SpriteRenderer> ();
		collidy = GetComponent<CircleCollider2D> ();
		activated = false;
		timer = score_timer;
	}

	public override void Activate(){
		Drop ();
	}

	void Update(){
		if (holder != null) {

            if (holder.GetComponent<PlayerController>().unconscious)
            {
                Drop();
                return;
            }

			timer -= Time.deltaTime;
			if (timer <= 0) {
				timer = score_timer;
				holder.GetComponent<Score_Counter> ().score += 1;
				holder.transform.Find ("Player 1 UI").transform.Find ("Coin Text").GetComponent<Text> ().text = holder.GetComponent<Score_Counter> ().score.ToString ();
			}
		} else {
			timer = score_timer;
		}
	}
}
