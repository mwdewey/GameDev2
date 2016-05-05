using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Curtain : MonoBehaviour
{
    public float transitionTime;

    private Image curtainImage;
    private float animTime;
    public bool isRunning;
    private bool isOpening;
    private Color32 curtainColor;
    private Animator anim;

	// Use this for initialization
	void Start ()
	{
	    isRunning = false;
        isOpening = true;

	    curtainImage = GetComponent<Image>();
        curtainColor = new Color32(0, 0, 0, 0);
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

    public void open()
    {
        isRunning = true;
        isOpening = true;
        anim.SetTrigger("open");
        anim.SetFloat("speed", transitionTime/2);
        
        Invoke("setISRunningFalse", transitionTime);
    }

    private void setISRunningFalse()
    {
        isRunning = false;
    }

    public void close()
    {
        isRunning = true;
        isOpening = false;
        anim.SetTrigger("close");
        anim.SetFloat("speed", transitionTime / 2);

        Invoke("setISRunningFalse", transitionTime);
    }

    public void instantOpen()
    {
        curtainImage.color = new Color32(0, 0, 0, 0); ;
    }

    public void instantClose()
    {
        curtainImage.color = new Color32(0, 0, 0, 255); ;
    }
}
