using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {


    private Image meter;
    private Color32 good_health = new Color32(0,255,0,255);
    private Color32 bad_health = new Color32(255,0,0,255);

    public float health;
    public float min_health;
    public float max_health;

    private float icon_min_width = 0;
    private float icon_max_width = 180;

	// Use this for initialization
	void Start () {

        meter = transform.Find("meter").gameObject.GetComponent<Image>();

	}
	
	// Update is called once per frame
	void Update () {

        Vector2 size = meter.rectTransform.sizeDelta;
        size.x = health / max_health * icon_max_width;
        meter.rectTransform.sizeDelta = size;

        Color newColor = Color.Lerp(bad_health,good_health,health / max_health);
        meter.color = newColor;

	}
}
