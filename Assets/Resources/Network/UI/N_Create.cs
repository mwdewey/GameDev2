using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class N_Create : MonoBehaviour
{
    public GameObject nameObject;
    public GameObject sizeObject;
    public GameObject publicObject;
    public GameObject successObject;

    public GameObject lobbyManagerObject;
    private N_LobbyManager lobbyManager;

    private Text nameText;
    private Text sizeText;
    private Text publicText;

    private List<MatchDesc> matchList = new List<MatchDesc>();
    private bool matchCreated;
    private NetworkMatch networkMatch;
    private CreateMatchRequest create;
    private NetworkManager nm;



    void Awake()
    {

        nameText = nameObject.GetComponent<Text>();
        sizeText = sizeObject.GetComponent<Text>();
        publicText = publicObject.GetComponent<Text>();

        nm = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
        if (nm.matchMaker == null) nm.StartMatchMaker();
        networkMatch = nm.matchMaker.GetComponent<NetworkMatch>();


    }

    public void createMatch()
    {
        create = new CreateMatchRequest();
        create.name = nameText.text;
        create.size = uint.Parse(sizeText.text);
        create.advertise = publicText.Equals("Yes");
        create.password = "";
        networkMatch.CreateMatch(create, OnMatchCreate);
    }

    public void OnMatchCreate(CreateMatchResponse matchResponse)
    {
        if (matchResponse.success)
        {
            successObject.SetActive(true);
            matchCreated = true;
            Utility.SetAccessTokenForNetwork(matchResponse.networkId, new NetworkAccessToken(matchResponse.accessTokenString));

            NetworkClient nc = nm.StartHost(new MatchInfo(matchResponse));
            nc.RegisterHandler(MsgType.Connect,OnConnected);
          
        }
        else print("Match create error!");
    }

    public void OnConnected(NetworkMessage msg)
    {

        SceneManager.LoadScene("network_test");

        print("Host connected");
    }

}