using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

    public GameObject controllerUI;
    private bool showUI;

	// Use this for initialization
	void Start () {

        showUI = true;

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.anyKeyDown && showUI)
        {
            showUI = false;
            gameObject.SetActive(false);
            controllerUI.SetActive(true);
        }




	}
}
