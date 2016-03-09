using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {

    private Image P1_icon;
    private Image P2_icon;
    private Image P3_icon;
    private Image P4_icon;
    private List<Image> icons;

    public float x_dist;
    public float y_dist;

    public GameObject P1_object;
    public GameObject P2_object;
    public GameObject P3_object;
    public GameObject P4_object;
    private List<GameObject> players;

    public GameObject main_player;

    private float map_width;
    private float map_height;

	// Use this for initialization
	void Start () {

        P1_icon = transform.Find("P1").gameObject.GetComponent<Image>();
        P2_icon = transform.Find("P2").gameObject.GetComponent<Image>();
        P3_icon = transform.Find("P3").gameObject.GetComponent<Image>();
        P4_icon = transform.Find("P4").gameObject.GetComponent<Image>();
        icons = new List<Image>();
        icons.Add(P1_icon);
        icons.Add(P2_icon);
        icons.Add(P3_icon);
        icons.Add(P4_icon);
		players = new List<GameObject>();

       


        map_width = GetComponent<RectTransform>().rect.width;
        map_height = GetComponent<RectTransform>().rect.height;

		tryFillPlayers ();

	}
	
	// Update is called once per frame
	void Update () {
		if (P1_object == null)
			tryFillPlayers ();
		else if (players == null) {
			players.Add(P1_object);
			players.Add(P2_object);
			players.Add(P3_object);
			players.Add(P4_object);
		}

        updateMiniMap();

	}
	
	void tryFillPlayers(){
		P1_object = GameObject.Find ("player_test_1(Clone)");
		P2_object = GameObject.Find ("player_test_2(Clone)");
		P3_object = GameObject.Find ("player_test_3(Clone)");
		P4_object = GameObject.Find ("player_test_4(Clone)");

		string pid = transform.parent.name;
		switch (pid) {
		case "Player 1 UI":
			main_player = GameObject.Find ("player_test_1(Clone)");
			break;
		case "Player 2 UI":
			main_player = GameObject.Find ("player_test_2(Clone)");
			break;
		case "Player 3 UI":
			main_player = GameObject.Find ("player_test_3(Clone)");
			break;
		case "Player 4 UI":
			main_player = GameObject.Find ("player_test_4(Clone)");
			break;
		}
	}


    void updateMiniMap()
    {
        GameObject targ = main_player;

		if (players.Count == 0) {
			return;
		}

        for (var i = 0; i < players.Count; i++ )
        {
            GameObject player = players[i];
            Image icon = icons[i];

            //if (player == targ) continue;

            if (Mathf.Abs(player.transform.position.x - targ.transform.position.x) < x_dist &&
                        Mathf.Abs(player.transform.position.y - targ.transform.position.y) < y_dist)
            {
                if (!icon.enabled) icon.enabled = true;

                // map difference of range, which is a radius around the target
                // value can be from -1 to 1, and map this to the width
                float mapped_x = (player.transform.position.x - targ.transform.position.x) / x_dist * map_width / 2;
                float mapped_y = (player.transform.position.y - targ.transform.position.y) / y_dist * map_height / 2;

                Vector3 pos = icon.transform.localPosition;

                pos.x = mapped_x;
                pos.y = mapped_y;

                icon.transform.localPosition = pos;
            }

            else if (icon.enabled) icon.enabled = false;



        }

        

    }
}
