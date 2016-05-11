using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinTextUpdater : MonoBehaviour {
	string last_value = "0";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (last_value != GetComponent<Text> ().text) {
			last_value = GetComponent<Text> ().text;
			if (int.Parse (last_value) < 0)
				GetComponent<Text> ().color = Color.yellow;
			else {
				GetComponent<Text> ().color = Color.white;
			}
		}
	}
}
