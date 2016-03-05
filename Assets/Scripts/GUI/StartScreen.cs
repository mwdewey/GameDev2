using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

    public GameObject controllerUI;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.anyKeyDown && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            controllerUI.SetActive(true);
        }




	}
}
