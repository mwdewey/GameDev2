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

    private GameObject[] staticObjects;


	// Use this for initialization
	void Start () {
        icons = new List<Image>();

        map_width = GetComponent<RectTransform>().rect.width;
        map_height = GetComponent<RectTransform>().rect.height;

        // minimap -> UI -> player
        main_player = transform.parent.parent.gameObject;

        staticObjects = GameObject.FindGameObjectsWithTag("Wall");
	}
	
	// Update is called once per frame
	void Update () {

        updateMiniMap(staticObjects);
        updateMiniMap(GameObject.FindGameObjectsWithTag("PlayerObject"));
        updateMiniMap(GameObject.FindGameObjectsWithTag("Fountain"));
        updateMiniMap(GameObject.FindGameObjectsWithTag("Portal"));

	}

    void addIcon()
    {
        GameObject obj = (GameObject)Instantiate(dotObject, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        Image ico = obj.GetComponent<Image>();
        ico.enabled = false;
        icons.Add(ico);
    }

    void updateMiniMap(GameObject[] objects)
    {

        for (var i = 0; i < objects.Length; i++)
        {
            GameObject player = objects[i];
            PlayerController pController = player.GetComponent<PlayerController>();


            if (icons.Count < i + 1) addIcon();
            Image icon = icons[i];


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

                icon.transform.localPosition = pos;

                
                switch(player.tag)
                {
                    case "PlayerObject":
                        icon.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        if (pController != null) icon.color = pController.playerColor;
                        else new Color32(255, 0, 0, 255);
                        break;
                    case "Fountain":
                        icon.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        icon.color = new Color32(3, 169, 244, 255);
                        break;
                    case "Floor":
                        icon.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                        icon.color = new Color32(100, 100, 100, 255);
                        break;
                    case "Wall":
                        icon.transform.localScale = new Vector3(0.075f, 0.075f, 0.075f);
                        icon.color = new Color32(100, 100, 100, 255);
                        break;
                    case "Portal":
                        icon.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        icon.color = new Color32(230,0,255, 255);
                        break;

                }

            }

            else if (icon.enabled) icon.enabled = false;



        }

        

    }
}
