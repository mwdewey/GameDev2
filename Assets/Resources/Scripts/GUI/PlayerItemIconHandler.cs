using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerItemIconHandler : MonoBehaviour {

    PlayerController pc;
    Image iconImage;

	// Use this for initialization
	void Start () {
        pc = transform.parent.parent.parent.GetComponent<PlayerController>();
        iconImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (pc.held_item == null && iconImage.sprite != null) {
			iconImage.sprite = null;
            iconImage.color = Color.black;
			return;
		}
		if (pc.held_item != null && iconImage.sprite == null) {
			Sprite currentSprite = pc.held_item.GetComponent<SpriteRenderer> ().sprite;
			iconImage.sprite = currentSprite;
            iconImage.color = Color.white;
		}

	}
}
