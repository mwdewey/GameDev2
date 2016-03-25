using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CustomNetworkManager : NetworkManager {
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);


        //NetworkServer.SpawnObjects();
    }

}
