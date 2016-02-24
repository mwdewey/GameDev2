﻿using UnityEngine;
using System.Collections;

public class ObjectLifetime : MonoBehaviour {
	/*Choose which way you want to define the object's lifetime by selecting or 
	 * deselecting dieAfterFrames. If it's set to false, the object will die 
	 * after the specified number of milliseconds instead. 
	 * */

	public int die_after_millis = 100;
	public int die_after_frames = 1;
	float start_time = 0;
	int start_frame = 0;
	public bool dieAfterFrames = false;

	// Use this for initialization
	void Start () {
		start_time = Time.time * 1000;
		start_frame = Time.frameCount;
	}
	
	// Update is called once per frame
	void Update () {
		if (dieAfterFrames) {
			if (Time.frameCount >= start_frame + die_after_frames) {
				Destroy (gameObject);
			} 
		}
		else {
			if (Time.time * 1000 >= start_time+die_after_millis) {
				Destroy (gameObject);
			}
		
		}
	}



}
