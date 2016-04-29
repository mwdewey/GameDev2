using UnityEngine;
using System.Collections;

public class PennyPincher: Item {
	public GameObject grabber;
	GameObject spr;
	float spr_scale_x;
	bool reverse = false;
	int lock_out = 8;
	Vector3 player_stays_here;
	public AudioClip grabber_sound;
	float grabber_speed = .1f;

	public override void Activate(){

        if (activated)
        {
            return;
        }

		holder.GetComponent<PlayerController> ().held_item = null;
		activated = true;
		spr = (GameObject) Instantiate (grabber, holder.transform.position, Quaternion.identity);
		spr.transform.parent = holder.transform;
		player_stays_here = holder.transform.position;
		spr_scale_x = grabber.transform.localScale.x;
		spr.transform.localScale = new Vector3(0f, spr.transform.localScale.y, 1f);
		spr.GetComponent<StealCoins> ().holder = holder;
		switch (holder.GetComponent<Animator>().GetInteger("DirectionState")){
		case 0: break; // Left
		case 1: spr.transform.Rotate(new Vector2(0f, 180f)); break;// Right
		case 2: spr.transform.Rotate(new Vector3(0f, 0f, -90f)); break;// Up
		case 3: spr.transform.Rotate(new Vector3(0f, 0f, 90f)); break;// Down
		}
		GetComponent<AudioSource> ().clip = grabber_sound;
		GetComponent<AudioSource> ().Play ();
	}
		

	void Update(){
		if (activated) {
			//holder.transform.position = player_stays_here;
			if (!reverse) {
				//spring out!
				if (spr.transform.localScale.x < spr_scale_x) {
					spr.transform.localScale = new Vector3 (spr.transform.localScale.x + grabber_speed, spr.transform.localScale.y, spr.transform.localScale.z);
				} 
				else {
					reverse = true;
				}
			} 
			else {
				//spring in!
				if (lock_out-- < 0) {
					if (spr.transform.localScale.x > 0) {
						spr.transform.localScale = new Vector3 (spr.transform.localScale.x - grabber_speed, spr.transform.localScale.y, spr.transform.localScale.z);
					} 
					else {
                        activated = false;
						Destroy (spr);
						Destroy (gameObject);
						return;
					}
				}
			}
		}
	}

	void FixedUpdate(){
		if (activated) {
			//holder.transform.position = player_stays_here;
		}
	}

}
