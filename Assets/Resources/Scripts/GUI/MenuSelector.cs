using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour {

    public List<GameObject> menuItems;
    public GameObject selectIconObject;
    public bool useCurtain = true;

    private GameObject selectIcon;
    private int currentSelected;
    private readonly float TIME_ACTION = 0.2f;
    private float timeSinceAction;
    private bool isMove;
    private Button.ButtonClickedEvent eventToFire;
    private Animator anim;
    private Curtain curtain;

    private AudioSource audioSource;
    public AudioClip audioClip;

	// Use this for initialization
	void Start () {

        currentSelected = 0;
        timeSinceAction = 0;

	    isMove = false;
	    eventToFire = null;
        curtain = transform.Find("Curtain").gameObject.GetComponent<Curtain>();
        audioSource = GetComponent<AudioSource>();

        selectIcon = Instantiate(selectIconObject);
        selectIcon.transform.SetParent(menuItems[0].transform);
        selectIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-25, 15, 0);
        selectIcon.transform.localScale = new Vector3(1, 1, 1);

        anim = selectIcon.GetComponent<Animator>();

        updateUI();
	}
	
	// Update is called once per frame
	void Update () {

        checkInput();
  
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
                audioSource.PlayOneShot(audioClip);
                anim.SetTrigger("activate");
                timeSinceAction = 0;
            }
        }

        // pressing menu item
        if ((Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return)) && !isMove)
        {
            isMove = true;
            eventToFire = selectIcon.transform.parent.gameObject.GetComponent<Button>().onClick;
            anim.SetTrigger("activate");
            if (useCurtain) curtain.close();
            timeSinceAction = 0;
        }

        if (isMove && !curtain.isRunning)
        {
            isMove = false;
            eventToFire.Invoke();
            if (useCurtain) curtain.instantOpen();
        }

        if (isMove && !useCurtain)
        {
            isMove = false;
            eventToFire.Invoke();
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


    }
   
}
