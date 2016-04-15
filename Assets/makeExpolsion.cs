using UnityEngine;
using System.Collections;

public class makeExpolsion : MonoBehaviour {

	public ParticleSystem EXPLKOSTION;
	ParticleSystem myExplostionw;

	// Use this for initialization
	void Start () {
		myExplostionw = EXPLKOSTION;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)){
			Instantiate(myExplostionw, transform.position, Quaternion.identity);
		}
	}
}
