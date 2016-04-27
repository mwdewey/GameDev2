using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour {

	public static int coins_remaining;
	float time_remaining;
	GameObject portal;
	bool portal_active = false;

	// Use this for initialization
	void Start () {
		time_remaining = 10f;
		coins_remaining = GameObject.FindGameObjectsWithTag ("Coin").Length;
		portal = GameObject.FindGameObjectWithTag ("Portal");
	}
	
	// Update is called once per frame
	void Update () {
		if (portal == null) {
			time_remaining = 10f;
			coins_remaining = GameObject.FindGameObjectsWithTag ("Coin").Length;
			portal = GameObject.FindGameObjectWithTag ("Portal");
		}
		if (!portal_active) {
			time_remaining -= Time.deltaTime;
			//Debug.Log (time_remaining);
			if (time_remaining <= 0 || coins_remaining <= 0) {
				portal_active = true;
				Color portal_color = portal.GetComponent<SpriteRenderer> ().color;
				portal.GetComponent<SpriteRenderer> ().color = new Color (portal_color.r, portal_color.g, portal_color.b, 1f);
				portal.GetComponent<PortalScript>().opened = true;
				//Debug.Log ("THE PORTAL HAS OPENED");
			}
		}
	}
}
