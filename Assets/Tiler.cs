using UnityEngine;
using System.Collections;

public class Tiler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.mainTextureScale = (new Vector2 (GetComponent<Transform> ().localScale.x, GetComponent<Transform> ().localScale.y));
	}
}
