using UnityEngine;
using System.Collections;

public class N_CameraSet : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();


	}
	
}
