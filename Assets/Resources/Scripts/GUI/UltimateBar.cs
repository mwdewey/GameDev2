using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UltimateBar : MonoBehaviour {

    private PlayerController pc;
    private Image meter;
    private Color32 meterColor;
    private float power;

    public float min_power;
    public float max_power;

    private float icon_max_width = 400;

	// Use this for initialization
	void Start () {
        pc = transform.parent.parent.GetComponent<PlayerController>();
        meter = transform.Find("meter").GetComponent<Image>();
        meter.color = pc.playerColor;
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 size = meter.rectTransform.sizeDelta;
        size.x = pc.power / max_power * icon_max_width;
        meter.rectTransform.sizeDelta = size;
	}
}
