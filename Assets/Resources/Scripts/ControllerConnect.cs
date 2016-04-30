using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControllerConnect : MonoBehaviour
{
    public GameObject prevMenu;

    private int controllerCount;

    private List<Color32> playerColors;
    private readonly Color32 P1_Color = new Color32(198, 40, 40, 255);
    private readonly Color32 P2_Color = new Color32(21, 101, 192, 255);
    private readonly Color32 P3_Color = new Color32(46, 125, 50, 255);
    private readonly Color32 P4_Color = new Color32(249, 168, 37, 255);

    private int characterCount = 4;

    private List<GameObject> csList;
    private List<List<GameObject>> csIconList;
    private List<GameObject> pbList;
    private List<GameObject> dbList;

    private List<CharacterObject> charList; 

    private GameObject cb;
    private List<PlayerLock> cbIconList;
    private readonly float TIME_TO_LOCK = 1;

    private GameObject cb_ready;
    private Text cb_ready_text;
    private readonly float START_TIME = 5;
    private float cb_ready_time;
    private bool start_initiated;
    private int currentStartSecond;

    private List<List<int>> characterSelections;
    private List<int> playerSelections;

    private List<float> timeSinceLastMovement;
    private readonly float TIME_TO_MOVE = 0.2f;

    private bool isBackPressed;
    private Curtain curtain;

	// Use this for initialization
	void Start ()
	{
        controllerCount = Input.GetJoystickNames().Length;

        playerColors = new List<Color32>();
        playerColors.Add(P1_Color);
        playerColors.Add(P2_Color);
        playerColors.Add(P3_Color);
        playerColors.Add(P4_Color);

        charList = new List<CharacterObject>();
        foreach(CharacterObject charObject in GameObject.FindObjectsOfType<CharacterObject>())
            charList.Add(charObject);

        csList = new List<GameObject>();
	    for (var i = 0; i < characterCount; i++)
	    {
	        GameObject characterSelectObject = transform.Find("CS " + (i + 1)).gameObject;
            csList.Add(characterSelectObject);

	        Text titleObject = characterSelectObject.transform.Find("title").gameObject.GetComponent<Text>();
	        titleObject.text = charList[i].name;
	    }

        csIconList = new List<List<GameObject>>();
        for (var i = 0; i < characterCount; i++)
        {
            csIconList.Add(new List<GameObject>());

            for (var i2 = 0; i2 < 4; i2++)
            {
                csIconList[i].Add(csList[i].transform.Find("icon " + (i2+1)).gameObject);
            }
        }

        pbList = new List<GameObject>();
        for (var i = 1; i < 5; i++) pbList.Add(transform.Find("PB " + i).gameObject);

        dbList = new List<GameObject>();
        for (var i = 1; i < 5; i++) dbList.Add(transform.Find("DB " + i).gameObject);

        cb = transform.Find("CB").gameObject;
        cbIconList = new List<PlayerLock>();
	    for (var i = 1; i < 5; i++)
	    {
	        Image img = cb.transform.Find("P" + i).gameObject.GetComponent<Image>();
            Image ico = cb.transform.Find("P" + i + " Check").gameObject.GetComponent<Image>();
	        img.color = playerColors[i-1];
            PlayerLock pl = new PlayerLock(img, ico, TIME_TO_LOCK);
            cbIconList.Add(pl);
	    }

        cb_ready = transform.Find("CB Ready").gameObject;
        cb_ready_text = cb_ready.transform.Find("Text").gameObject.GetComponent<Text>();
	    cb_ready_time = 0;
	    start_initiated = false;

        characterSelections = new List<List<int>>();
        for (var i = 0; i < characterCount; i++) characterSelections.Add(new List<int>());
        for (var i = 0; i < controllerCount; i++) characterSelections[0].Add(i);

        playerSelections = new List<int>();
        for (var i = 0; i < controllerCount; i++) playerSelections.Add(0);

        timeSinceLastMovement = new List<float>();
        for (var i = 0; i < controllerCount; i++) timeSinceLastMovement.Add(0);

	    isBackPressed = false;
        curtain = transform.Find("Curtain").gameObject.GetComponent<Curtain>();

        generateCharacterUI();
	    generatePlayerUI();

        setCharSelect(0, 0);
        setCharSelect(1, 0);
        setCharSelect(2, 0);
        setCharSelect(3, 0);

	}
	
	void Update ()
	{
        moveSelections();
        checkBackPressed();
        animateTest();

        if (isBackPressed && !curtain.isRunning)
        {
            isBackPressed = false;
            gotoMainMenu();
            curtain.instantOpen();
        }
	}

    void generateCharacterUI()
    {
        for (var i = 0; i < characterCount; i++)
        {
            GameObject cs = csList[i];
            List<int> currentSelected = characterSelections[i];

            for (var i2 = 0; i2 < 4; i2++)
            {
                GameObject csIcon = cs.transform.Find("icon " + (i2+1)).gameObject;
                Image csIconImage = csIcon.GetComponent<Image>();

                if (currentSelected.Count > i2) csIconImage.color = playerColors[currentSelected[i2]];
                else csIconImage.color = new Color32(158,158,158,255);
            }


        }
    }

    void generatePlayerUI()
    {
		for (var i = 0; i < 4; i++)
        {
            GameObject pb = pbList[i];
            GameObject db = dbList[i];

            Image pbImage = pb.GetComponent<Image>();
            Image dbImage = db.GetComponent<Image>();

            Image portraitImg = pb.transform.Find("Image").gameObject.GetComponent<Image>();
            Text charDescription = db.transform.Find("text").gameObject.GetComponent<Text>();
            Animator portraitAnim = portraitImg.GetComponent<Animator>();

            if (controllerCount > i)
            {
                CharacterObject charObj = charList[playerSelections[i]];
                pbImage.color = playerColors[i];
                dbImage.color = playerColors[i];
                portraitAnim.runtimeAnimatorController = charObj.portrait;
                charDescription.text = charObj.description;
                portraitAnim.SetFloat("animSpeed", charObj.port_speed);
            }

            else {

                GameObject cb = transform.Find("CB").gameObject;
                GameObject cb_p = cb.transform.Find("P" + (i + 1)).gameObject;
                GameObject cb_check = cb.transform.Find("P" + (i + 1) + " Check").gameObject;

                pb.SetActive(false);
                db.SetActive(false);
                cb_p.SetActive(false);
                cb_check.SetActive(false);
            }

        }
    }

    void checkBackPressed()
    {

        for (var i = 0; i < controllerCount; i++)
        {

            if (Input.GetKeyDown("joystick " + (i + 1) + " button 1"))
            {
                // normal back
                if (!cbIconList[i].IsLocked)
                {
                    isBackPressed = true;
                    curtain.close();
                }

                // free player lock
                else
                {
                    PlayerLock playerLock = cbIconList[i];

                    playerLock.IsLocked = false;
                    playerLock.LockTime = 0;

                    // if start is initiated, reverse it
                    if (start_initiated)
                    {
                        start_initiated = false;
                        cb_ready.SetActive(false);
                    }
                }
            }
        }
    }

    void moveSelections()
    {

        for (var i = 0; i < controllerCount; i++)
        {
            if (timeSinceLastMovement[i] > TIME_TO_MOVE)
            {

                int currentPosition = playerSelections[i];
                int prevPosition = currentPosition;

                // move down
				if (Input.GetAxis("Joy" + (i + 1) + "_LeftStickVertical") < -0.5f && 
                    !cbIconList[i].IsLocked &&
                    !Input.GetKey("joystick " + (i + 1) + " button 0"))
                {
                    if (currentPosition == (characterCount - 1)) currentPosition = 0;
                    else currentPosition++;
                }

                // move up
				else if (Input.GetAxis("Joy" + (i + 1) + "_LeftStickVertical") > 0.5f &&
                    !cbIconList[i].IsLocked &&
                    !Input.GetKey("joystick " + (i + 1) + " button 0"))
                {
                    if (currentPosition == 0) currentPosition = (characterCount - 1);
                    else currentPosition--;
                }

                // if position changed
                if (prevPosition != currentPosition)
                {
                    characterSelections[prevPosition].Remove(i);
                    playerSelections[i] = currentPosition;
                    characterSelections[currentPosition].Add(i);

                    setCharSelect(i,currentPosition);
                    generateCharacterUI();
                    generatePlayerUI();

                    timeSinceLastMovement[i] = 0;
                }

            }

            timeSinceLastMovement[i] += Time.deltaTime;
        }

    }

    public void onClick(String gameMode)
    {
        gameObject.SetActive(true);
        prevMenu.SetActive(false);
    }

    void gotoMainMenu()
    {
        gameObject.SetActive(false);
        prevMenu.SetActive(true);
    }

    void setCharSelect(int PID,int CharID)
    {
        PlayerPrefCodes playerCode = 0;
        CharCodes charCode = 0;

        switch (PID)
        {
            case 0: playerCode = PlayerPrefCodes.Player1CharSelect; break;
            case 1: playerCode = PlayerPrefCodes.Player2CharSelect; break;
            case 2: playerCode = PlayerPrefCodes.Player3CharSelect; break;
            case 3: playerCode = PlayerPrefCodes.Player4CharSelect; break;
        }

        string charName = charList[CharID].name;

        switch (charName)
        {
            case "Shifter": charCode = CharCodes.Shifter; break;
            case "Miss Q":  charCode = CharCodes.MissQ;   break;
            case "Vegano":  charCode = CharCodes.Vegano;  break;
            case "Rich":    charCode = CharCodes.Rich;    break;
        }

        PlayerPrefs.SetInt(playerCode.ToString(), (int) charCode);

    }

    void animateTest()
    {
        for (var i = 0; i < controllerCount; i++)
        {
            if (timeSinceLastMovement[i] < TIME_TO_MOVE && Time.fixedTime > 1)
            {
                int iconIndex = characterSelections[playerSelections[i]].IndexOf(i);
                float scale = timeSinceLastMovement[i] / TIME_TO_MOVE;
                csIconList[playerSelections[i]][iconIndex].transform.localScale = new Vector3(scale, scale, 1);

            }

            // player lock code

            PlayerLock playerLock = cbIconList[i];
            Vector3 iconScale = playerLock.LockBackground.transform.localScale;

            if (Input.GetKey("joystick " + (i + 1) + " button 0"))
            {
                if (!playerLock.IsLocked)
                {
                    playerLock.LockTime += Time.deltaTime;

                    if (playerLock.LockTime >= playerLock.TimeToLock)
                    {
                        playerLock.IsLocked = true;
                        iconScale.y = 1;
                        playerLock.LockIcon.color = new Color32(33,33,33,255);
                        playerLock.LockBackground.transform.localScale = iconScale;
                    }
                }
            }
                
            else
            {
                if (playerLock.LockTime > 0 && !playerLock.IsLocked)
                {
                    playerLock.LockTime -= Time.deltaTime;
                }

                if (playerLock.LockTime < 0) playerLock.LockTime = 0;
            }

            if (!playerLock.IsLocked)
            {
                iconScale.y = playerLock.LockTime/playerLock.TimeToLock;
                playerLock.LockBackground.transform.localScale = iconScale;
            }

        }

        // game start code
        if (!start_initiated)
        {
            bool isAllLocked = true;
            for (var i = 0; i < controllerCount; i++) if (!cbIconList[i].IsLocked) isAllLocked = false;

            if (isAllLocked)
            {
                start_initiated = true;
                cb_ready.SetActive(true);
                cb_ready_time = 0;
                currentStartSecond = (int)(START_TIME - cb_ready_time) + 1;
                cb_ready_text.text = "Starting in " + currentStartSecond;
            }
        }

        else
        {
            if (cb_ready_time > START_TIME)
            {
                // start dungeon
                cb_ready_text.text = "Starting...";
                SceneManager.LoadScene("split_screen_test");
                curtain.instantOpen();
            }

            else
            {
                int tempCurSec = currentStartSecond;
                cb_ready_time += Time.deltaTime;
                currentStartSecond = (int) (START_TIME - cb_ready_time) + 1;

                // if second changes, update text
                if (tempCurSec != currentStartSecond)
                {
                    cb_ready_text.text = "Starting in " + currentStartSecond;
                }

                // 
                if (!curtain.isRunning && curtain.transitionTime > (START_TIME - cb_ready_time))
                {
                    curtain.close();
                }

            }
        }

    }

}

public class Player
{
    String PID;
    PlayerLock playerLock;



}

public class PlayerLock
{
    public Image LockBackground;
    public Image LockIcon;
    public float TimeToLock;
    public float LockTime;
    public bool IsLocked;

    public PlayerLock(Image lockBackground, Image lockIcon,float timeToLock)
    {
        LockBackground = lockBackground;
        LockIcon = lockIcon;
        TimeToLock = timeToLock;

        LockTime = 0;
        IsLocked = false;
    }

}

