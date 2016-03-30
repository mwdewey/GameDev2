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

	public int power = 0;
	private CharCodes character;

	void Start () {
		character = gameObject.GetComponent<PlayerController>().character;
	}
	
	void Update () {
	
	}

    public void handleUltimateInput() {
        if (power >= 0) {
			switch (character) {
			case CharCodes.MissQ:
				gameObject.GetComponent<MissQ_Ultimate> ().ultimate ();
				break;
			case CharCodes.Shifter:
				gameObject.GetComponent<Shifter_Ultimate> ().ultimate ();
				break;
			}
        }
    }

}
