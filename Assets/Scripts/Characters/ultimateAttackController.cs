using UnityEngine;
using System.Collections;

public class ultimateAttackController : MonoBehaviour {

    /*
     * HOW TO USE THIS CONTROLLER:
     *  Make a prefab for each character, and give it a particle system and ultimate attack script. 
     *     For instance, "missQ.prefab" contains "missQUlt.psystem" and "missQUlt.cs". 
     *  This script will be attached to a player, and will be called in playerController. When the 
     *     "ultimate" button is pressed, this script will be invoked, check if the ultimate can happen,
     *     and execute it appropriately.
     */

    public GameObject ultimatePrefab;

    private int power;

	void Start () {
	
	}
	
	void Update () {
	
	}

    public void handleUltimateInput() {
        if (power >= 100) {

        }
    }

    private void executeAbility() {

    }
}
