using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlintAndSteel : Item {

	int ammo;
	public GameObject fire;

	public override void Activate () {
		if (!activated) {
			ammo = 6;
			activated = true;
		}
		if (ammo > 0) {
			ammo -= 1;
			int direction = holder.GetComponent<Animator> ().GetInteger ("DirectionState");
            GameObject fireObject = null;
			switch (direction) {
			case 0:
                fireObject = (GameObject) Instantiate(fire, holder.transform.position + new Vector3(-1.0f, 0.0f, 0.0f), Quaternion.identity);
				break; // Left
			case 1:
                fireObject = (GameObject) Instantiate(fire, holder.transform.position + new Vector3(1.0f, 0.0f, 0.0f), Quaternion.identity);
				break; // Right
			case 2:
                fireObject = (GameObject) Instantiate(fire, holder.transform.position + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
				break; // Up
			case 3:
                fireObject = (GameObject) Instantiate(fire, holder.transform.position + new Vector3(0.0f, -1.0f, 0.0f), Quaternion.identity);
				break; // Down
			}

            if (fireObject != null)
            {
                fireObject.GetComponent<Fire>().source = holder;
            }

			if (ammo == 0) {
				holder.GetComponent<PlayerController> ().held_item = null;
				Destroy (gameObject);
			}
		}
	}
}
