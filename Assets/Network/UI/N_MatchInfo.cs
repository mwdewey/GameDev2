using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class N_MatchInfo : MonoBehaviour {

    public bool isConnected = false;
    private Text label;

	// Use this for initialization
	void Start () {

        label = transform.Find("Text").gameObject.GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void updateConnected(bool _isConnected)
    {
        isConnected = _isConnected;

        label.text = "Connected: " + isConnected.ToString();
    }
}
