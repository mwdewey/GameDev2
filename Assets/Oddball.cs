using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Oddball : Item {

	public float score_timer;
	float timer;
	public GameObject particles;
	public GameObject my_ps;

	// Use this for initialization
	void Start () {
		ring = GetComponentsInChildren<SpriteRenderer> () [1];
		rendy = GetComponent<SpriteRenderer> ();
		collidy = GetComponent<CircleCollider2D> ();
		activated = false;
		timer = score_timer;
	}

	public override void Activate(){
		if(my_ps!=null) Destroy(my_ps);
		Drop ();
	}

	void Update(){
		if (holder != null) {
			if (my_ps == null) {
				GameObject p = (GameObject) Instantiate (particles, holder.transform.position, Quaternion.identity);
				p.transform.SetParent (holder.transform);
				my_ps = p;
			}

            if (holder.GetComponent<PlayerController>().unconscious)
            {
				if (my_ps!=null) Destroy (my_ps);
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

	public override void Drop(){
		if (my_ps!=null) Destroy (my_ps);
		base.Drop ();
	}
}
