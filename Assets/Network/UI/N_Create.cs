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

    private Text nameText;
    private Text sizeText;
    private Text publicText;

    private List<MatchDesc> matchList = new List<MatchDesc>();
    private bool matchCreated;
    private NetworkMatch networkMatch;
    private CreateMatchRequest create;



    void Awake()
    {

        nameText = nameObject.GetComponent<Text>();
        sizeText = sizeObject.GetComponent<Text>();
        publicText = publicObject.GetComponent<Text>();

        networkMatch = gameObject.AddComponent<NetworkMatch>();

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
            NetworkManager nm = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
            NetworkClient nc = nm.StartClient(new MatchInfo(matchResponse));
            nc.RegisterHandler(MsgType.Connect, OnConnected);


            int id = 5;
            //N_LobbyManager lm = GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<N_LobbyManager>();
            //lm.addPlayer(id);

            // update match info
            GameObject m_info_obj = GameObject.FindGameObjectWithTag("Match Info");
            m_info_obj.GetComponent<N_MatchInfo>().updateConnected(true);

            //SceneManager.LoadScene("network_test");
           

        }
        else print("Match create error!");
    }

    public void OnConnected(NetworkMessage msg)
    {
        Debug.Log("Connected!");
    }
}