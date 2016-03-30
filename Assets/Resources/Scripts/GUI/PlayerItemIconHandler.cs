using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerItemIconHandler : MonoBehaviour {

    PlayerController pc;
    Image iconImage;
    Sprite heldSprite;

	// Use this for initialization
	void Start () {
        pc = transform.parent.parent.parent.GetComponent<PlayerController>();
        iconImage = GetComponent<Image>();
        if (pc.held_item != null)
        {
            heldSprite = pc.held_item.GetComponent<SpriteRenderer>().sprite;
            iconImage.sprite = heldSprite;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (pc.held_item == null || heldSprite == null) return;

        Sprite currentSprite = pc.held_item.GetComponent<SpriteRenderer>().sprite;
        if (currentSprite.name != heldSprite.name)
        {
            iconImage.sprite = currentSprite;
            heldSprite = currentSprite;
        }

	}
}
