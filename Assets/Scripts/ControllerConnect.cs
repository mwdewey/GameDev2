using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class ControllerConnect : MonoBehaviour
{
    private Regex joystickRegex;
    private List<int> controllerIds;
    private int controllerCount;
    private readonly int MAX_PLAYERS = 4;
    private readonly Color32 ACTIVE_COLOR = new Color32(56, 142, 60, 255);
    private readonly Color32 INACTIVE_COLOR = new Color32(211, 47, 47, 255);

    private List<Color32> playerColors;
    private readonly Color32 P1_Color = new Color32(244, 67, 54, 255);
    private readonly Color32 P2_Color = new Color32(33,150,243, 255);
    private readonly Color32 P3_Color = new Color32(76, 175, 80, 255);
    private readonly Color32 P4_Color = new Color32(255, 235, 59, 255);


    private List<GameObject> csList;
    private List<GameObject> pbList;
    private List<GameObject> dbList;
    private GameObject cb;

    private List<List<int>> characterSelections;
    private List<int> playerSelections;

    private List<float> timeSinceLastMovement; 

	// Use this for initialization
	void Start ()
	{
        joystickRegex = new Regex(@"Joystick([0-9]+)Button([0-9]+)");
        controllerIds = new List<int>();
        controllerCount = Input.GetJoystickNames().Length;

        playerColors = new List<Color32>();
        playerColors.Add(P1_Color);
        playerColors.Add(P2_Color);
        playerColors.Add(P3_Color);
        playerColors.Add(P4_Color);

        csList = new List<GameObject>();
        for (var i = 1; i < 5; i++) csList.Add(transform.Find("CS " + i).gameObject);

        pbList = new List<GameObject>();
        for (var i = 1; i < 5; i++) pbList.Add(transform.Find("PB " + i).gameObject);

        dbList = new List<GameObject>();
        for (var i = 1; i < 5; i++) dbList.Add(transform.Find("DB " + i).gameObject);

        cb = transform.Find("CB").gameObject;

        characterSelections = new List<List<int>>();
        for (var i = 0; i < 4; i++) characterSelections.Add(new List<int>());
        for (var i = 0; i < controllerCount; i++) characterSelections[0].Add(i);

        playerSelections = new List<int>();
        for (var i = 0; i < controllerCount; i++) playerSelections.Add(0);

        timeSinceLastMovement = new List<float>();
        for (var i = 0; i < controllerCount; i++) timeSinceLastMovement.Add(0);

        generateCharacterUI();
	    generatePlayerUI();

	    updateUI();
	}
	
	// Update is called once per frame
	void Update ()
	{

	    detectControllers();
        moveSelections();

        if (Input.GetKeyDown(KeyCode.A)) generateCharacterUI();

	}

    void generateCharacterUI()
    {
        for (var i = 0; i < 4; i++)
        {
            GameObject cs = csList[i];
            List<int> currentSelected = characterSelections[i];

            for (var i2 = 0; i2 < 4; i2++)
            {
                GameObject csIcon = cs.transform.Find("icon " + (i2+1)).gameObject;
                Image csIconImage = csIcon.GetComponent<Image>();

                if (currentSelected.Count > i2) csIconImage.color = playerColors[currentSelected[i2]];
                else csIconImage.color = Color.white;
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

            if (controllerIds.Count > i)
            {
                pbImage.color = playerColors[i];
                dbImage.color = playerColors[i];
            }

            else {
                pbImage.color = new Color32(33, 33, 33, 255);
                dbImage.color = new Color32(33, 33, 33, 255);
            }

        }
    }


    void detectControllers()
    {

        if (Input.anyKeyDown)
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    String buttonString = kcode.ToString();

                    Match match = joystickRegex.Match(buttonString);

                    // if a key is pressed on a controller/joystick
                    if (match.Success)
                    {
                        int controllerId = Int32.Parse(match.Groups[1].Value);
                        int controllerInput = Int32.Parse(match.Groups[2].Value);

                        controllerDetected(controllerId, controllerInput);
                        Debug.logger.Log(controllerId + " " + controllerInput);
                    }

                    else Debug.logger.Log(buttonString);

                }
            }
        }


    }


    void controllerDetected(int controllerId,int buttonId)
    {
        // 0 is a, 1 is b for Xbox controller
        switch (buttonId)
        {
            case 0 : addController(controllerId); break;
            case 1 : removeController(controllerId); break;
        }


    }

    void addController(int controllerId)
    {

        // only add new controller if it doesn't exist and player count is < MAX_PLAYERS
        if (!controllerIds.Contains(controllerId) && controllerIds.Count < MAX_PLAYERS)
        {
            controllerIds.Add(controllerId);
            Debug.logger.Log("Player " + getPlayerIndex(controllerId) + " added");
            updateUI();
        }


    }

    void removeController(int controllerId)
    {
        // only remove if it exists
        if (controllerIds.Contains(controllerId))
        {
            Debug.logger.Log("Player " + getPlayerIndex(controllerId) + " removed");
            controllerIds.Remove(controllerId);
            updateUI();
        }

    }

    int getPlayerIndex(int controllerId)
    {
        return controllerIds.IndexOf(controllerId);
    }

    void updateUI()
    {

        Debug.logger.Log("Count: " + controllerIds.Count);
    }

    void moveSelections()
    {

        for (var i = 0; i < 4; i++)
        {

            if (Input.GetButtonDown(" "))
            {

            }


        }

    }


}

