using UnityEngine;
using System.Collections;

public class Consciousness : MonoBehaviour {
	/*The purpose of this class is not only to keep track of the player's
	 * consciousness and put them in a knocked out state when it is down to
	 * 0, but also to take damage from other scripts.  
	*/

	int CONSCIOUSNESS = 50; //this is the test constant. Change this to affect the staring consciousness value.
	public int consciousness; //this is the actual consciousness.

	// Use this for initialization
	void Start () {
		consciousness = CONSCIOUSNESS;
	}
	
	// Update is called once per frame
	void Update () {
		if (consciousness <= 0) {
			//agh!
		}
	}

	public void TakeDamage(int damage){
		consciousness -= damage;
		print ("ouch! consciousness at "+(((float)consciousness)/CONSCIOUSNESS)*100+"%");
	}
}
