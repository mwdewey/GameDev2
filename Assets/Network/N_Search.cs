using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using System.Collections.Generic;

public class N_Search : MonoBehaviour
{

    public GameObject searchParent;
    public GameObject matchObject;

    private List<MatchDesc> matchList = new List<MatchDesc>();
    private NetworkMatch networkMatch;
    private MenuSelector menuSelector;

    void Awake()
    {
        networkMatch = gameObject.AddComponent<NetworkMatch>();
        menuSelector = GetComponent<MenuSelector>();

        networkMatch.ListMatches(0, 20, "", OnMatchList);
    }

    public void refresh()
    {
        networkMatch.ListMatches(0, 20, "", OnMatchList);
    }

    private void refreshUI()
    {

        // destroy old match objects
        List<GameObject> children = new List<GameObject>();
        foreach (Transform c in searchParent.transform) children.Add(c.gameObject);
        foreach (GameObject c in children) Destroy(c);

        // add new ones
        Vector3 match_pos = new Vector3(0,0,0);
        float y_inc = -40;
        menuSelector.menuItems.RemoveRange(1, menuSelector.menuItems.Count-1);
        foreach (MatchDesc desc in matchList)
        {
            GameObject match_temp = (GameObject) Instantiate(matchObject, match_pos, Quaternion.identity);
            match_temp.transform.SetParent(searchParent.transform,false);
            MatchObject m_obj = match_temp.GetComponent<MatchObject>();

            // set attributes of the match
            m_obj.nameText.text = desc.name;
            m_obj.playerText.text = desc.currentSize.ToString();

            // link to menu selector object
            menuSelector.menuItems.Add(m_obj.nameObject);

            // create button onclick action
            Button b_temp = m_obj.nameObject.GetComponent<Button>();
            b_temp.onClick.AddListener(delegate { buttonClicked(desc); });

            match_pos.y += y_inc;
        }

    }

    public void OnMatchList(ListMatchResponse matchListResponse)
    {
        matchList = matchListResponse.matches;

        refreshUI();
    }

    public void OnMatchJoined(JoinMatchResponse matchJoin)
    {
        if (matchJoin.success)
        {
            Debug.Log("Join match succeeded");
            Utility.SetAccessTokenForNetwork(matchJoin.networkId, new NetworkAccessToken(matchJoin.accessTokenString));
            NetworkManager.singleton.StartClient(new MatchInfo(matchJoin));

            // update match info
            GameObject m_info_obj = GameObject.FindGameObjectWithTag("Match Info");
            m_info_obj.GetComponent<N_MatchInfo>().updateConnected(true);
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

    public void OnError(NetworkMessage msg)
    {
        print("Network error: " + msg.reader.ReadString());
    }

    public void buttonClicked(MatchDesc desc)
    {
        networkMatch.JoinMatch(desc.networkId, "", OnMatchJoined);

    }
}