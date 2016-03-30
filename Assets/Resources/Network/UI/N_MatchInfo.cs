using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class N_MatchInfo : MonoBehaviour {

    public bool isConnected = false;
    private Text label;

	// Use this for initialization
	void Start () {

        label = transform.Find("ConnectText").gameObject.GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {

        if (label != null) label.text = "Connected: " + isConnected.ToString();

	}


    public void updateConnected(bool _isConnected)
    {
        isConnected = _isConnected;

        label.text = "Connected: " + isConnected.ToString();
    }
}
