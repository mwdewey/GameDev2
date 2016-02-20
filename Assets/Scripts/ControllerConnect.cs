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

    private GameObject player_1_ui;
    private GameObject player_2_ui;
    private GameObject player_3_ui;
    private GameObject player_4_ui;

    private Image player_1_icon;
    private Image player_2_icon;
    private Image player_3_icon;
    private Image player_4_icon;


	// Use this for initialization
	void Start ()
	{
        joystickRegex = new Regex(@"Joystick([0-9]+)Button([0-9]+)");

        controllerIds = new List<int>();

        player_1_ui = GameObject.Find("Player 1");
        player_2_ui = GameObject.Find("Player 2");
        player_3_ui = GameObject.Find("Player 3");
        player_4_ui = GameObject.Find("Player 4");

        player_1_icon = player_1_ui.transform.FindChild("connected icon").gameObject.GetComponent<Image>();
        player_2_icon = player_2_ui.transform.FindChild("connected icon").gameObject.GetComponent<Image>();
        player_3_icon = player_3_ui.transform.FindChild("connected icon").gameObject.GetComponent<Image>();
        player_4_icon = player_4_ui.transform.FindChild("connected icon").gameObject.GetComponent<Image>();

	    updateUI();
	}
	
	// Update is called once per frame
	void Update ()
	{

	    DetectControllers();

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
        if (controllerIds.Count > 0) player_1_icon.color = ACTIVE_COLOR;
        else player_1_icon.color = INACTIVE_COLOR;

        if (controllerIds.Count > 1) player_2_icon.color = ACTIVE_COLOR;
        else player_2_icon.color = INACTIVE_COLOR;

        if (controllerIds.Count > 2) player_3_icon.color = ACTIVE_COLOR;
        else player_3_icon.color = INACTIVE_COLOR;

        if (controllerIds.Count > 3) player_4_icon.color = ACTIVE_COLOR;
        else player_4_icon.color = INACTIVE_COLOR;

        Debug.logger.Log("Count: " + controllerIds.Count);
    }


}

