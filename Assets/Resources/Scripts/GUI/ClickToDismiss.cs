using UnityEngine;
using System.Collections;
using System;

public class ClickToDismiss : MonoBehaviour
{

    public GameObject prevMenu;
    private bool isBackPressed;
    private Curtain curtain;

	// Use this for initialization
	void Start () {
        isBackPressed = false;
        curtain = transform.Find("Curtain").gameObject.GetComponent<Curtain>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKeyDown && !isBackPressed)
	    {
	        isBackPressed = true;
            if (curtain != null) curtain.close();
	    }

	    if (isBackPressed && !curtain.isRunning)
	    {
	        isBackPressed = false;
            prevMenu.SetActive(true);
	        gameObject.SetActive(false);
            if (curtain != null) curtain.instantOpen();
	    }

	    
        
	}

    public void ButtonClicked()
    {
        gameObject.SetActive(true);
        prevMenu.SetActive(false);
    }
}
