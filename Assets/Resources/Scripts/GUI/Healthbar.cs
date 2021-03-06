﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {


    private Image meter;
    private Color32 good_health = new Color32(0,255,0,255);
    private Color32 mid_health = new Color32(255, 255, 0, 255);
    private Color32 bad_health = new Color32(255,0,0,255);

    public float health;
    public float min_health;
    public float max_health;

    private float icon_max_width;

	// Use this for initialization
	void Start () {

        GameObject meterObj = transform.Find("meter").gameObject;

        meter = meterObj.GetComponent<Image>();
        icon_max_width = meterObj.GetComponent<RectTransform>().rect.width;

	}
	
	// Update is called once per frame
	void Update () {

		transform.parent.parent.GetComponent<PlayerController> ().health = health;

        Vector2 size = meter.rectTransform.sizeDelta;
        size.x = health / max_health * icon_max_width;
        meter.rectTransform.sizeDelta = size;

        Color newColor;
        if(health < max_health/2) newColor = Color.Lerp(bad_health, mid_health, health / (max_health / 2));
        else newColor = Color.Lerp(mid_health, good_health, (health - max_health / 2)/ (max_health / 2));
        meter.color = newColor;


	}
}
