using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class init_spawner : MonoBehaviour {



	public GameObject template;
	private CharCodes character;
	public RuntimeAnimatorController missq, shifter, rich, vegano;

    public bool isDebug = false;

	// Use this for initialization
	void Start () {
        int controllerCount = Input.GetJoystickNames().Length;

        if (isDebug)
        {
            PlayerPrefs.SetInt(PlayerPrefCodes.Player1CharSelect.ToString(),(int) CharCodes.Shifter);
            controllerCount = 1;
        }

        for (int i = 0; i < controllerCount; i++)
        {
			
			GameObject p = (GameObject) Instantiate (template, transform.position, Quaternion.identity);
			p.AddComponent<ultimateAttackController> ();
			switch (i) {
			case 0:
				p.name = "kdaddy";
				p.GetComponent<PlayerController> ().PID = "1";
				p.GetComponent<PlayerController>().playerColor = new Color32(244, 67, 54, 255);
				character = (CharCodes) PlayerPrefs.GetInt (PlayerPrefCodes.Player1CharSelect.ToString ());    
				p.GetComponent<CameraController> ().cameraObject = GameObject.Find ("TopLeft");
				p.transform.Find ("Player 1 UI").gameObject.GetComponent<Canvas> ().worldCamera = GameObject.Find ("TopLeft").GetComponent<Camera>();
				break;
			case 1:
				p.name = "d-diddy";
				p.GetComponent<PlayerController> ().PID = "2";
				p.GetComponent<PlayerController>().playerColor = new Color32(33, 150, 243, 255);
				character = (CharCodes) PlayerPrefs.GetInt (PlayerPrefCodes.Player2CharSelect.ToString ());  
				p.GetComponent<CameraController> ().cameraObject = GameObject.Find ("TopRight");
				p.transform.Find ("Player 1 UI").gameObject.GetComponent<Canvas> ().worldCamera = GameObject.Find ("TopRight").GetComponent<Camera>();
				break;
			case 2:
				p.name = "trumpster";
				p.GetComponent<PlayerController> ().PID = "3";
				p.GetComponent<PlayerController>().playerColor = new Color32(76, 175, 80, 255);
				character = (CharCodes) PlayerPrefs.GetInt (PlayerPrefCodes.Player3CharSelect.ToString ());  
				p.GetComponent<CameraController> ().cameraObject = GameObject.Find ("BottomLeft");
				p.transform.Find ("Player 1 UI").gameObject.GetComponent<Canvas> ().worldCamera = GameObject.Find ("BottomLeft").GetComponent<Camera>();
				break;
			case 3:
				p.name = "LOOK AT ME I'M A EASTER EGG";
				p.GetComponent<PlayerController> ().PID = "4";
				p.GetComponent<PlayerController>().playerColor = new Color32(255, 235, 59, 255);
				character = (CharCodes) PlayerPrefs.GetInt (PlayerPrefCodes.Player4CharSelect.ToString ());  
				p.GetComponent<CameraController> ().cameraObject = GameObject.Find ("BottomRight");
				p.transform.Find ("Player 1 UI").gameObject.GetComponent<Canvas> ().worldCamera = GameObject.Find ("BottomRight").GetComponent<Camera>();
				break;
			}
			p.GetComponent<PlayerController> ().character = character;
			//Debug.Log (character);
			switch(character) {
			case CharCodes.MissQ:
				p.AddComponent<MissQ_Ultimate> ();
				p.GetComponent<Animator> ().runtimeAnimatorController = missq;
                p.GetComponent<Animator>().SetFloat("rangeSpeed", 0.5f);
                p.GetComponent<Animator>().SetFloat("meleeSpeed", 0.5f);
				break;
			case CharCodes.Shifter:
				p.AddComponent<Shifter_Ultimate> ();
				p.GetComponent<Animator> ().runtimeAnimatorController = shifter;
                p.GetComponent<Animator>().SetFloat("rangeSpeed", 3);
                p.GetComponent<Animator>().SetFloat("meleeSpeed", 3);
                break;
			case CharCodes.Rich:
				p.AddComponent<Rich_Ultimate> ();
				p.GetComponent<Animator> ().runtimeAnimatorController = rich;
				break;
			case CharCodes.Vegano:
				p.AddComponent<Vegano_Ultimate> ();
				p.GetComponent<Animator> ().runtimeAnimatorController = vegano;
				break;
			}
		}
	}
}
