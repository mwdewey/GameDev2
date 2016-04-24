using UnityEngine;
using UnityEngine.UI;
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
	GameObject coin;
	PlayerController pc;

	public AudioClip unconscious_sound;

	// Use this for initialization
	void Start () {
		pc = GetComponent<PlayerController>();
		healthBar = (Healthbar) transform.Find("Player 1 UI").Find("Healthbar").gameObject.GetComponent<Healthbar>();
		initial_health = healthBar.health;
		coin = (GameObject)Resources.Load ("Prefabs/Environment/Coin");
	}
	
	// Update is called once per frame
	void Update () {
		if (healthBar.health <= 0 && !pc.unconscious) {
			GetComponent<Animator> ().SetInteger ("PlayerState", 0);
			pc.unconscious = true;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			GetComponent<AudioSource> ().clip = unconscious_sound;
			GetComponent<AudioSource> ().Play ();

			int coins_lost = GetComponent<Score_Counter> ().score / 2;
			GetComponent<Score_Counter> ().score /= 2;

			transform.Find ("Player 1 UI").transform.Find("Coin Text").gameObject.GetComponent<Text>().text = GetComponent<Score_Counter> ().score.ToString();

			Manager.coins_remaining += coins_lost;
			for (int x = 0; x < coins_lost; x++) {
				Instantiate (coin, new Vector3 (Random.Range (-1f, 1f), Random.Range (-1f, 1f), 0) + transform.position, Quaternion.identity);
			}

			StartCoroutine(GetUp());
		}
	}

	public void TakeDamage(int damage){
		if (!pc.unconscious) {
			healthBar.health -= damage;
		}
	}

	public void Revive(){
		healthBar.health = initial_health;
	}

	IEnumerator GetUp(){
		yield return new WaitForSeconds (5);
		if (GetComponent<Animator> ().GetInteger ("PlayerState") != 1) { 
			GetComponent<Animator> ().SetInteger ("PlayerState", 2);
			healthBar.health = initial_health;
			if (!pc.locked) {
				pc.unconscious = false;
			}
		}
	}
}
