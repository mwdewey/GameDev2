using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class DeathByFloor : MonoBehaviour {
	/*Put this script on any floor to instantly make it
	 * A FLOOR OF DEATH!! You choose in this script how long 
	 * the player will take to respawn because this script is 
	 * in control of that too! OnTriggerEnter2D replaces the
	 * player with the Sprite dead (publicly assigned, see 
	 * below), and then starts a coroutine that counts down
	 * to the respawn and then makes it happen.
	*/

	int millis_until_respawn = 3000;
	int closest_distance;

    GameObject coin;

	public Sprite dead;		//this is the dead version of the player. Code can be added to pick which sprite that is later on

	public AudioClip death_sound;
	public AudioClip respawn_sound;

    void Start()
    {
        coin = (GameObject)Resources.Load("Prefabs/Environment/Coin");
    }

	void OnTriggerEnter2D(Collider2D c){
		if (c.gameObject.tag == "Ring") {
			//You can do whatever you want to the player who just collided by using c.gameObject.whatever. 
			//death time!
			GameObject c2 = c.transform.parent.gameObject;
			c2.gameObject.GetComponent<Animator>().SetInteger("PlayerState",1);
			GetComponent<AudioSource> ().clip = death_sound;
			GetComponent<AudioSource> ().Play ();
			c2.gameObject.GetComponent<PlayerController> ().unconscious = true;//stop movement
			c2.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			c2.gameObject.GetComponent<PlayerController> ().setKnockBack (null);

            int coins_lost = c2.GetComponent<Score_Counter>().score / 2;
            c2.GetComponent<Score_Counter>().score /= 2;

            c2.transform.Find("Player 1 UI").transform.Find("Coin Text").gameObject.GetComponent<Text>().text = c2.GetComponent<Score_Counter>().score.ToString();

            Manager.coins_remaining += coins_lost;
            for (int x = 0; x < coins_lost; x++)
            {
                GameObject coin_obj = (GameObject) Instantiate(coin, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) + c2.transform.position, Quaternion.identity);

                coin_obj.GetComponent<Animator>().SetFloat("offset", Random.Range(0, 1f));

            }

			  //make sure they don't just keep moving on the velocity they already had
			StartCoroutine (RespawnCountdown( c2.gameObject )); 
			  //countdown to respawn
		}
	}

	IEnumerator RespawnCountdown(GameObject g){
		yield return new WaitForSeconds(millis_until_respawn/1000f); 

		//pick the closest available respawn point
		List<GameObject> all_respawn_pts = GameObject.FindGameObjectsWithTag ("Respawn").ToList();
		//print (all_respawn_pts.Count);
		GameObject closest_spawn_pt = null;
		for (int i = 0; i < all_respawn_pts.Count; i++) {
			float dis = Vector3.Distance (all_respawn_pts [i].transform.position, transform.position);
			if (i == 0 || dis < closest_distance) {
                closest_distance = (int) dis;
				closest_spawn_pt = all_respawn_pts [i];
			}
		}

		if (all_respawn_pts.Count>0) g.transform.position = new Vector3(closest_spawn_pt.transform.position.x, closest_spawn_pt.transform.position.y+.6f, closest_spawn_pt.transform.position.z);
		g.GetComponent<PlayerController> ().unconscious = false;
		g.GetComponent<Consciousness> ().Revive ();
		GetComponent<AudioSource> ().clip = respawn_sound;
		GetComponent<AudioSource> ().Play ();
		g.GetComponent<Animator>().SetInteger("PlayerState",2);
		//  ^put stuff back the way it was

	}
}