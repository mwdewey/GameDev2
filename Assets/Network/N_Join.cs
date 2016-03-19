using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using System.Collections.Generic;

public class N_Join : MonoBehaviour
{

    public GameObject nameObject;
    public GameObject joinSuccessObject;

    private Text nameText;

    private bool matchFound = false;
    private List<MatchDesc> matchList = new List<MatchDesc>();
    private NetworkMatch networkMatch;

    void Awake()
    {

        nameText = nameObject.GetComponent<Text>();

        networkMatch = gameObject.AddComponent<NetworkMatch>();
    }

    public void joinMatch()
    {
        networkMatch.ListMatches(0, 20, "", OnMatchList);
    }

    public void OnMatchList(ListMatchResponse matchListResponse)
    {

        foreach (MatchDesc desc in matchListResponse.matches)
        {
            if (desc.name.Equals(nameText.text))
            {
                networkMatch.JoinMatch(desc.networkId, "", OnMatchJoined);
                matchFound = true;
                break;
            }
        }

        if(!matchFound) print("Match could not be found");

    }

    public void OnMatchJoined(JoinMatchResponse matchJoin)
    {
        if (matchJoin.success)
        {
            print("Match found");
            joinSuccessObject.SetActive(true);
            Utility.SetAccessTokenForNetwork(matchJoin.networkId, new NetworkAccessToken(matchJoin.accessTokenString));
            NetworkClient myClient = new NetworkClient();
            myClient.RegisterHandler(MsgType.Connect, OnConnected);
            myClient.Connect(new MatchInfo(matchJoin));
        }
        else
        {
            Debug.LogError("Join match failed");
        }
    }

    public void OnConnected(NetworkMessage msg)
    {
        Debug.Log("Connected!");
    }
}