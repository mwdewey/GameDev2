using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerItemIconHandler : MonoBehaviour {

    PlayerController pc;
    Image iconImage;
    Vector2 default_size;

	// Use this for initialization
	void Start () {
        pc = transform.parent.parent.parent.GetComponent<PlayerController>();
        iconImage = GetComponent<Image>();
        default_size = iconImage.rectTransform.sizeDelta;
	}
	
	// Update is called once per frame
	void Update () {
		if (pc.held_item == null && iconImage.sprite != null) {
			iconImage.sprite = null;
            iconImage.color = new Color32(78, 52, 46, 255);
            iconImage.transform.localScale = new Vector3(1, 1, 1);
            iconImage.rectTransform.sizeDelta = default_size;
		}
		else if (pc.held_item != null && iconImage.sprite == null) {
			Sprite currentSprite = pc.held_item.GetComponent<SpriteRenderer> ().sprite;
            
			iconImage.sprite = currentSprite;
            iconImage.color = Color.white;

            iconImage.SetNativeSize();
            iconImage.transform.localScale = pc.held_item.transform.localScale;
		}

	}
}
