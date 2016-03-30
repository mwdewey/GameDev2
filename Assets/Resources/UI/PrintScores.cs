using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrintScores : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int p1 = PlayerPrefs.GetInt("p" + 0 + "score");
        int p2 = PlayerPrefs.GetInt("p" + 1 + "score");
        int p3 = PlayerPrefs.GetInt("p" + 2 + "score");
        int p4 = PlayerPrefs.GetInt("p" + 3 + "score");

        string scoreText = "Score\nPlayer 1: " + p1 + "\nPlayer 2: " + p2 + "\nPlayer 3: " + p3 + "\nPlayer 4: " + p4;

        GetComponent<Text>().text = scoreText;
	}
	
}
