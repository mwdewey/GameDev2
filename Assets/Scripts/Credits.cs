using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

    public GameObject mainMenu;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }
	}

    public void ButtonClicked()
    {
        Debug.logger.Log("Credits started");
        gameObject.SetActive(true);
        mainMenu.SetActive(false);
    }
}
