using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class SimpleLobbyManager : NetworkBehaviour
{
    [SyncVar]
    public int count;

    private Text playerCount;

    void Start()
    {
        playerCount = GameObject.FindGameObjectWithTag("Match Info").transform.Find("PlayerCountText").gameObject.GetComponent<Text>();
    }

    void Update()
    {
        playerCount.text = "count: " + count;
    }

    [Server]
    public void Cmd_spawn(GameObject g){
        NetworkServer.SpawnObjects();
    }

}
