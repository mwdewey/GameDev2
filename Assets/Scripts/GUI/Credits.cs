﻿using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

    public GameObject mainMenu;
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
	        curtain.close();
	    }

	    if (isBackPressed && !curtain.isRunning)
	    {
	        isBackPressed = false;
	        gameObject.SetActive(false);
            mainMenu.SetActive(true);
            curtain.instantOpen();
	    }

	    
        
	}

    public void ButtonClicked()
    {
        Debug.logger.Log("Credits started");
        gameObject.SetActive(true);
        mainMenu.SetActive(false);
    }
}