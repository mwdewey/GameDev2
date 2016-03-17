using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour {

    public List<GameObject> menuItems;
    public GameObject selectIcon;

    private int currentSelected;
    private readonly float TIME_ACTION = 0.2f;

    private float timeSinceAction;

    private bool isMove;
    private Button.ButtonClickedEvent eventToFire;
    private Curtain curtain;

	// Use this for initialization
	void Start () {

        currentSelected = 0;
        timeSinceAction = 0;

	    isMove = false;
	    eventToFire = null;
        curtain = transform.Find("Curtain").gameObject.GetComponent<Curtain>();

        updateUI();
	}
	
	// Update is called once per frame
	void Update () {

        checkInput();

        animate();

        timeSinceAction += Time.deltaTime;

	}

    void checkInput()
    {

        // moving between menu items
        if (timeSinceAction > TIME_ACTION && !isMove)
        {
            int tempSelected = currentSelected;

            // move down
            if (Input.GetAxis("Vertical") < -0.5f || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentSelected == (menuItems.Count - 1)) currentSelected = 0;
                else currentSelected++;
            }

            // move up
            else if (Input.GetAxis("Vertical") > 0.5f || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentSelected == 0) currentSelected = (menuItems.Count - 1);
                else currentSelected--;
            }

            if (tempSelected != currentSelected)
            {
                updateUI();

                timeSinceAction = 0;
            }
        }

        // pressing menu item
        if ((Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return)) && !isMove)
        {
            isMove = true;
            eventToFire = selectIcon.transform.parent.gameObject.GetComponent<Button>().onClick;
            curtain.close();
        }

        if (isMove && !curtain.isRunning)
        {
            isMove = false;
            eventToFire.Invoke();
            curtain.instantOpen();
        }


    }

    void checkMouse()
    {
        if (Input.mousePresent)
        {



        }
    }

    void updateUI()
    {
        GameObject menuItem = menuItems[currentSelected];

        selectIcon.transform.SetParent(menuItem.transform,false);

        Button menuButton = menuItem.GetComponent<Button>();
        Image imageButton = menuItem.GetComponent<Image>();

    }

    void animate()
    {
        if (timeSinceAction < TIME_ACTION && Time.time > 1)
        {
            float scale = (timeSinceAction / TIME_ACTION)/2 + 0.5f;
            selectIcon.transform.localScale = new Vector3(scale, scale, 1);

            scale = (timeSinceAction / TIME_ACTION);

            if (scale < 0.5f)
            {
                selectIcon.transform.parent.Find("Text").localPosition = new Vector3(-scale * 20, 0, 0);
            }

            else
            {
                selectIcon.transform.parent.Find("Text").localPosition = new Vector3(-(1-scale) * 20, 0, 0);
            }

        }


    }
}
