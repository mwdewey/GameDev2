using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PortalBar : MonoBehaviour
{

    private PlayerController pc;
    private Image meter;
    private Color32 meterColor;

    public float power;
    public float min_power;
    public float max_power;

    private float icon_max_width = 400;

    private Manager manager;

    // Use this for initialization
    void Start()
    {
        pc = transform.parent.parent.GetComponent<PlayerController>();
        meter = transform.Find("meter").GetComponent<Image>();

        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 size = meter.rectTransform.sizeDelta;
        float percentage = (60 - manager.time_remaining) / 60 * 100;
        size.x = percentage / max_power * icon_max_width;
        meter.rectTransform.sizeDelta = size;
    }
}
