using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {

    public float x_dist;
    public float y_dist;
    public GameObject dotObject;
    private List<Image> icons;
    private GameObject main_player;
    private float map_width;
    private float map_height;


	// Use this for initialization
	void Start () {

        icons = new List<Image>(); for (int i = 0; i < 16; i++)
        {
            GameObject obj = (GameObject)Instantiate(dotObject,Vector3.zero,Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(0.1f,0.1f,1f);
            Image ico = obj.GetComponent<Image>();
            ico.enabled = false;
            icons.Add(ico);
        }

        map_width = GetComponent<RectTransform>().rect.width;
        map_height = GetComponent<RectTransform>().rect.height;

        // minimap -> UI -> player
        main_player = transform.parent.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {

        updateMiniMap();

	}

    void updateMiniMap()
    {

        GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerObject");

        print(players.Length);

        for (var i = 0; i < players.Length; i++ )
        {
            GameObject player = players[i];
            PlayerController pController = player.GetComponent<PlayerController>();
            Image icon = icons[i];

            print(player.name);

            if (Mathf.Abs(player.transform.position.x - main_player.transform.position.x) < x_dist &&
                        Mathf.Abs(player.transform.position.y - main_player.transform.position.y) < y_dist)
            {
                if (!icon.enabled) icon.enabled = true;

                // map difference of range, which is a radius around the target
                // value can be from -1 to 1, and map this to the width
                float mapped_x = (player.transform.position.x - main_player.transform.position.x) / x_dist * map_width / 2;
                float mapped_y = (player.transform.position.y - main_player.transform.position.y) / y_dist * map_height / 2;

                Vector3 pos = icon.transform.localPosition;

                pos.x = mapped_x;
                pos.y = mapped_y;

                //print(pos.x + " " + pos.y);

                icon.transform.localPosition = pos;
                icon.color = pController.playerColor;
            }

            else if (icon.enabled) icon.enabled = false;



        }

        

    }
}
