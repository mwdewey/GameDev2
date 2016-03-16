using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using UnityEngine.UI;

public class N_InfoMenu : MonoBehaviour {

    private NetworkPlayer netp;
    public GameObject textObject;
    private Text textArea;

    string testStatus = "Testing network connection capabilities.";
    string testMessage = "Test in progress";
    string shouldEnableNatMessage = "";
    bool doneTesting = false;
    bool probingPublicIP = false;
    int serverPort = 9999;
    bool useNat;
    float timer = 0;

    ConnectionTesterStatus connectionTestResult = ConnectionTesterStatus.Undetermined;

	// Use this for initialization
	void Start () {
        textArea = textObject.GetComponent<Text>();
        netp = Network.player;
	}
	
	// Update is called once per frame
	void Update () {

        if (!doneTesting)
        {
            TestConnection();
            UpdateUI();
        }

	}

    void UpdateUI()
    {
        string displayText = "";
        displayText += "External IP Address: " + netp.externalIP + "\n";
        displayText += "External Port: " + netp.externalPort + "\n";
        displayText += "Internal IP Address: " + netp.ipAddress + "\n";
        displayText += "Internal Port: " + netp.port + "\n";
        displayText += "\n";
        displayText += "Connection Status: " + testStatus + "\n";
        displayText += "Connection Test: " + testMessage + "\n";
        displayText += "Should Enable Test: " + shouldEnableNatMessage + "\n";
        displayText += "IPTest: " + "\n";

        textArea.text = displayText;
    }

    void TestConnection()
    {
        // Start/Poll the connection test, report the results in a label and 
        // react to the results accordingly
        connectionTestResult = Network.TestConnection();
        switch (connectionTestResult)
        {
            case ConnectionTesterStatus.Error:
                testMessage = "Problem determining NAT capabilities";
                doneTesting = true;
                break;

            case ConnectionTesterStatus.Undetermined:
                testMessage = "Undetermined NAT capabilities";
                doneTesting = false;
                break;

            case ConnectionTesterStatus.PublicIPIsConnectable:
                testMessage = "Directly connectable public IP address.";
                useNat = false;
                doneTesting = true;
                break;

            // This case is a bit special as we now need to check if we can 
            // circumvent the blocking by using NAT punchthrough
            case ConnectionTesterStatus.PublicIPPortBlocked:
                testMessage = "Non-connectable public IP address (port " +
                    serverPort + " blocked), running a server is impossible.";
                useNat = false;
                // If no NAT punchthrough test has been performed on this public 
                // IP, force a test
                if (!probingPublicIP)
                {
                    connectionTestResult = Network.TestConnectionNAT();
                    probingPublicIP = true;
                    testStatus = "Testing if blocked public IP can be circumvented";
                    timer = Time.time + 10;
                }
                // NAT punchthrough test was performed but we still get blocked
                else if (Time.time > timer)
                {
                    probingPublicIP = false; 		// reset
                    useNat = true;
                    doneTesting = true;
                }
                break;

            case ConnectionTesterStatus.PublicIPNoServerStarted:
                testMessage = "Test Failed";
                break;

            case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
                testMessage = "Limited";
                useNat = true;
                doneTesting = true;
                break;

            case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
                testMessage = "Limited";
                useNat = true;
                doneTesting = true;
                break;

            case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
            case ConnectionTesterStatus.NATpunchthroughFullCone:
                testMessage = "Open";
                useNat = true;
                doneTesting = true;
                break;

            default:
                testMessage = "Error in test routine, got " + connectionTestResult;
                break;
        }

        if (doneTesting)
        {

            if (useNat)
                shouldEnableNatMessage = "Yes";
            else
                shouldEnableNatMessage = "No";
            testStatus = "Done testing";
        }
    }
}
