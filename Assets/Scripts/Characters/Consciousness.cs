using UnityEngine;
using System.Collections;

public class Consciousness : MonoBehaviour {
	/*The purpose of this class is not only to keep track of the player's
	 * consciousness and put them in a knocked out state when it is down to
	 * 0, but also to take damage from other scripts.  
	*/

	//int CONSCIOUSNESS = 50; //this is the test constant. Change this to affect the staring consciousness value.
	public int consciousness; //this is the actual consciousness.
	public GameObject myHealthBar;
	private Healthbar healthBar;

	// Use this for initialization
	void Start () {
		healthBar = myHealthBar.GetComponent<Healthbar> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (healthBar.health <= 0) {
			//agh!
		}
	}

	public void TakeDamage(int damage){
		healthBar.health -= damage;
		print ("ouch! consciousness at "+(((float)consciousness)/healthBar.max_health)*100+"%");
	}
}
