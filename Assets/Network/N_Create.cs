using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
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
            NetworkManager.singleton.StartClient(new MatchInfo(matchResponse));


            // update match info
            GameObject m_info_obj = GameObject.FindGameObjectWithTag("Match Info");
            m_info_obj.GetComponent<N_MatchInfo>().updateConnected(true);


        }
        else print("Match create error!");
    }

}