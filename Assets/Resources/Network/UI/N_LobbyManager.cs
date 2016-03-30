using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class N_LobbyManager : NetworkBehaviour
{

    private Text playerCountText;

    public struct PlayerState
    {
        public int id;
        public int net_id;

        public PlayerState(int id, int net_id)
        {
            this.id = id;
            this.net_id = net_id;
        }
    };
    public class SyncPlayerState : SyncListStruct<PlayerState> { }

    private SyncPlayerState playerStates = new SyncPlayerState();

    void Awake()
    {
        playerStates.Callback = OnPSChanged;
        playerCountText = GameObject.FindGameObjectWithTag("Match Info").transform.Find("PlayerCountText").gameObject.GetComponent<Text>();
    }

    public void addPlayer(int id)
    {
        PlayerState ps = new PlayerState(playerStates.Count,id);
        playerStates.Add(ps);
        playerCountText.text = "Players: " + playerStates.Count;
    }

    public void removePlayer(int id)
    {
        // code
    }

    private void OnPSChanged(SyncListStruct<PlayerState>.Operation op, int index)
    {
        Debug.Log("list changed " + op);
    }
}
