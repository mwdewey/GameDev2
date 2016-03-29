using UnityEngine;
using System.Collections;

public class Consciousness : MonoBehaviour {
	/*The purpose of this class is not only to keep track of the player's
	 * consciousness and put them in a knocked out state when it is down to
	 * 0, but also to take damage from other scripts.  
	*/

	//int CONSCIOUSNESS = 50; //this is the test constant. Change this to affect the staring consciousness value.
	private GameObject myHealthBar;
	private Healthbar healthBar;
	private float initial_health;

	// Use this for initialization
	void Start () {
		string pid = GetComponent<PlayerController> ().PID;
        healthBar = (Healthbar) transform.Find("Player 1 UI").Find("Healthbar").gameObject.GetComponent<Healthbar>();
		initial_health = healthBar.health;
	}
	
	// Update is called once per frame
	void Update () {
		if (healthBar.health <= 0 && !GetComponent<PlayerController>().unconscious) {
			GetComponent<Animator> ().SetInteger ("PlayerState", 0);
			GetComponent<PlayerController>().unconscious = true;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			GetComponent<Attacks>().unconscious = true;
			StartCoroutine(GetUp());
		}
	}

	public void TakeDamage(int damage){
		healthBar.health -= damage;
		//print ("ouch! consciousness at "+(((float)healthBar.health)/healthBar.max_health)*100+"%");
	}

	IEnumerator GetUp(){
		yield return new WaitForSeconds (5);
		GetComponent<Animator> ().SetInteger ("PlayerState", 2);
		GetComponent<PlayerController> ().unconscious = false;
		GetComponent<Attacks> ().unconscious = false;
		healthBar.health = initial_health;
	}
}
