using UnityEngine;
using System.Collections;

public class ultimateAttackController : MonoBehaviour {
    /*
     * HOW TO USE THIS CONTROLLER:
     * 
     * 	  YOUR SCRIPT **MUST** HAVE A FUNCTION CALLED "ultimate()". This is what this script calls to
     *      actually execute the attack.
     * 
     *    This script will be attached to a player, and will be called in playerController. When the 
     *     "ultimate" button is pressed, this script will be invoked, check if the ultimate can happen,
     *     and execute it appropriately.
     */

    public GameObject ultimatePrefab;
	private int power = 0;
	public MonoBehaviour Script;
	private string character;


	void Start () {
		character = gameObject.GetComponent<PlayerController>().character;
	}
	
	void Update () {
	
	}

    public void handleUltimateInput() {
        if (power >= 100) {
			//gameObject.GetComponent (character + "_Ultimate").ultimate();
        }
    }

}
