using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Curtain : MonoBehaviour
{
    private Image curtainImage;
    public float transitionTime;
    private float animTime;

    public bool isRunning;
    private bool isOpening;

    private Color32 curtainColor;

	// Use this for initialization
	void Start ()
	{

	    animTime = 0;
	    isRunning = false;
        isOpening = true;

	    curtainImage = GetComponent<Image>();

        curtainColor = new Color32(0, 0, 0, 0);

	}
	
	// Update is called once per frame
	void Update ()
	{
	    animateCurtain();

        if(Input.GetKeyDown(KeyCode.A)) open();
        if (Input.GetKeyDown(KeyCode.S)) close();

	}

    void animateCurtain()
    {
        if (isRunning)
        {
            if (isOpening)
            {
                float alpha = (transitionTime - animTime)/transitionTime*255;

                if (alpha <= 0)
                {
                    isRunning = false;
                    curtainColor.a = 0;
                }
                else curtainColor.a = (byte)Convert.ToInt32(alpha);
            }

            else
            {
                float alpha = animTime / transitionTime * 255;

                if (alpha >= 255)
                {
                    isRunning = false;
                    curtainColor.a = 255;
                }
                else curtainColor.a = (byte)Convert.ToInt32(alpha);
            }

            curtainImage.color = curtainColor;
            animTime += Time.deltaTime;
        }
    }

    public void open()
    {
        isRunning = true;
        animTime = 0;
        isOpening = true;

    }

    public void close()
    {
        isRunning = true;
        animTime = 0;
        isOpening = false;
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
