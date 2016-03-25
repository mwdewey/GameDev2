using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class N_Spawner : MonoBehaviour {

    GameObject o;
    CustomNetworkManager nm;

	// Use this for initialization
	void Start () {

        o = GameObject.FindGameObjectWithTag("NetworkManager");
        nm = o.GetComponent<CustomNetworkManager>();

        ClientScene.Ready(nm.client.connection);
        ClientScene.AddPlayer(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
