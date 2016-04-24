using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAnimation : MonoBehaviour {

    private Image img;
    private SpriteRenderer rend;

    private string name;

	// Use this for initialization
	void Start () {

        img  = GetComponent<Image>();
        rend = GetComponent<SpriteRenderer>();

        name = "";
	}
	
	// Update is called once per frame
	void Update () {

        if (rend.sprite != null)
        {
            if (!name.Equals(rend.sprite.name) || true)
            {
                img.sprite = rend.sprite;
                name = rend.sprite.name;
            }
        }

        

	}
}
