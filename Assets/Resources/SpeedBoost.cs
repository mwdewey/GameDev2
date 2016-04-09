using UnityEngine;
using System.Collections;

public class SpeedBoost : Item {

	float active_lifetime;
	Transform t;
	Transform holder_t;

	public override void Activate (){
		holder.GetComponent<PlayerController> ().speed_boost = 1.5f;
		active_lifetime = 20f;
		activated = true;
		t = GetComponent<Transform> ();
		holder_t = holder.GetComponent<Transform> ();
		rendy.enabled = true;
	}

	void Update(){
		if (activated) {
			active_lifetime -= Time.deltaTime;
			t.position = holder_t.position;
			if (active_lifetime <= 0) {
				holder.GetComponent<PlayerController> ().speed_boost = 1.0f;
				Destroy (gameObject);
			}
		}
	}
}
