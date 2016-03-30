using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalScript : MonoBehaviour {

	public bool opened;
	float timer;

	bool vortex = false;
	int total_players;
	GameObject[] players;

	// Use this for initialization
	void Start () {
		opened = false;
		timer = 30f;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "PlayerObject" && opened) {
			other.GetComponentsInParent<Score_Counter> ()[0].in_portal = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "PlayerObject" && opened) {
			other.GetComponentsInParent<Score_Counter> ()[0].in_portal = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (opened) {
			timer -= Time.deltaTime;
			if (timer <= 0 || Input.GetKeyDown(KeyCode.P)) {
				opened = false;
				GetComponent<SpriteRenderer> ().enabled = false;
				Debug.Log ("The portal has been closed");
				//shut down all the player colliders
				players = GameObject.FindGameObjectsWithTag ("PlayerObject");
				total_players = players.Length;
				for (int i = 0; i < players.Length; i++) {
					players [i].GetComponent<Collider2D> ().enabled = false;
					players [i].GetComponentInChildren<PolygonCollider2D> ().enabled = false;
					players [i].GetComponent<PlayerController> ().enabled = false;
					vortex = true;
				}

                // ends the game 10 seconds after the vortex is active
                Invoke("endGame", 10);
			}
		}
		if (vortex) {
			for (int p =0; p < players.Length; p++) {
				players[p].transform.position = Vector2.MoveTowards (players [p].transform.position, transform.position, .1f); 
			}
		}
	}

    void endGame()
    {
        players = GameObject.FindGameObjectsWithTag("PlayerObject");
        for (int i = 0; i < players.Length; i++)
        {
            PlayerPrefs.SetInt("p" + i + "score", players[i].GetComponent<Score_Counter>().score);
        }

        SceneManager.LoadScene("end_scene");
    }
}
