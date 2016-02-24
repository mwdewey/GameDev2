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
    private readonly int MAX_PLAYERS = 4;
    private readonly Color32 ACTIVE_COLOR = new Color32(56, 142, 60, 255);
    private readonly Color32 INACTIVE_COLOR = new Color32(211, 47, 47, 255);

    private Vector2 icon_margin = new Vector2(20,20);

    public GameObject charSelectIcon_object;
    private List<GameObject> charSelectIconList;
    public int numOfChars = 8;

    public GameObject playerIcon_object;
    private List<GameObject> playerIconList;
    private float playerIconWidth = 132.5f;

    public GameObject descriptionBox_object;
    private List<GameObject> descriptionBoxList;

    public GameObject continueButton_object;
    private GameObject continueButtonInstance;
    private float continueButtonHeight = 40;

	// Use this for initialization
	void Start ()
	{
        joystickRegex = new Regex(@"Joystick([0-9]+)Button([0-9]+)");
        controllerIds = new List<int>();

        charSelectIconList = new List<GameObject>();
        playerIconList = new List<GameObject>();
        descriptionBoxList = new List<GameObject>();

        generateCharacterUI();
	    generatePlayerUI();

	    updateUI();
	}
	
	// Update is called once per frame
	void Update ()
	{

	    DetectControllers();

        if (Input.GetKeyDown(KeyCode.A)) generateCharacterUI();

	}

    void generateCharacterUI()
    {

        foreach (GameObject g in charSelectIconList) Destroy(g);
        charSelectIconList.Clear();

        Vector2 win_dim = GetComponent<RectTransform>().sizeDelta;

        for (var i = 0; i < numOfChars; i++)
        {
            GameObject uiIcon = Instantiate(charSelectIcon_object);
            uiIcon.transform.parent = transform;

            RectTransform rectTransform = uiIcon.GetComponent<RectTransform>();
            float height = (win_dim.y - icon_margin.y * (numOfChars + 1)) / numOfChars;
            rectTransform.sizeDelta = new Vector2(150, height);
            rectTransform.anchoredPosition = new Vector2(95, -height * (i + 0.5f) - icon_margin.y * (i + 1));
            rectTransform.localScale = new Vector3(1, 1, 1);

            charSelectIconList.Add(uiIcon);
        }

    }

    void generatePlayerUI()
    {
        //int playerCount = controllerIds.Count;
        int playerCount = 4;
        Vector2 win_dim = GetComponent<RectTransform>().sizeDelta;
        float height = (win_dim.y - icon_margin.y*4 - continueButtonHeight)/2;

        foreach (GameObject g in playerIconList) Destroy(g);
        playerIconList.Clear();
        foreach (GameObject g in descriptionBoxList) Destroy(g);
        descriptionBoxList.Clear();

        // create player selected character UIs and descriptions
        for (var i = 0; i < playerCount; i++)
        {
            GameObject playerIcon = Instantiate(playerIcon_object);
            playerIcon.transform.parent = transform;

            RectTransform rectTransform = playerIcon.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(playerIconWidth, height);
            rectTransform.anchoredPosition = new Vector2(190+playerIconWidth*(0.5f + i) + icon_margin.x*i, -icon_margin.y - height/2);
            rectTransform.localScale = new Vector3(1, 1, 1);

            playerIconList.Add(playerIcon);

            GameObject descriptionBox = Instantiate(descriptionBox_object);
            descriptionBox.transform.parent = transform;

            rectTransform = descriptionBox.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(playerIconWidth, height);
            rectTransform.anchoredPosition = new Vector2(190 + playerIconWidth * (0.5f + i) + icon_margin.x * i, -icon_margin.y * 2 - height * 1.5f);
            rectTransform.localScale = new Vector3(1, 1, 1);

            descriptionBoxList.Add(descriptionBox);

        }

        // create continue button
        Destroy(continueButtonInstance);
        continueButtonInstance = Instantiate(continueButton_object);
        continueButtonInstance.transform.parent = transform;

        RectTransform conTransform = continueButtonInstance.GetComponent<RectTransform>();
        float continueButtonWidth = win_dim.x - 210;
        conTransform.sizeDelta = new Vector2(continueButtonWidth, continueButtonHeight);
        conTransform.anchoredPosition = new Vector2(190 + continueButtonWidth / 2, -win_dim.y + icon_margin.y + continueButtonHeight / 2);
        conTransform.localScale = new Vector3(1, 1, 1);

    }


    void DetectControllers()
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


}

