﻿using UnityEngine;
using System.Collections;

public class VeganoParticlesTurn : MonoBehaviour {

	public int rotate_speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate( new Vector3(0, 0, rotate_speed));
	}
}
