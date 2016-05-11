using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PortalScript : MonoBehaviour {

	public bool opened;
    public float timer;

	bool vortex = false;
	GameObject[] players;
	public List<Sprite> sprites;
	int which_sprite;
	SpriteRenderer rendy;

	public AudioClip portal_open_sound;

	// Use this for initialization
	void Start () {
		opened = false;
		timer = 30f;
		which_sprite = 0;
		rendy = GetComponent<SpriteRenderer> ();
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

	public void Open(){
		opened = true;
		which_sprite++;
		rendy.sprite = sprites [which_sprite];
		StartCoroutine (PortalAnimation ());
	}
	
	// Update is called once per frame
	void Update () {
		if (opened) {
			timer -= Time.deltaTime;
			if (timer <= 0 || Input.GetKeyDown(KeyCode.P)) {
				opened = false;
				Debug.Log ("The portal has been closed");
				StopCoroutine ("PortalAnimation");
				rendy.sprite = sprites [1];
				//shut down all the player colliders
				players = GameObject.FindGameObjectsWithTag ("PlayerObject");
				for (int i = 0; i < players.Length; i++) {
					players [i].GetComponent<Collider2D> ().enabled = false;
                    players[i].GetComponent<PlayerController>().unconscious = true;
					players [i].GetComponentInChildren<PolygonCollider2D> ().enabled = false;
					players [i].GetComponent<PlayerController> ().enabled = false;
                    players[i].GetComponent<Rigidbody2D>().isKinematic = true;
					vortex = true;
                    PlayerPrefs.SetInt("p" + players[i].GetComponent<PlayerController>().PID + "score", players[i].GetComponent<Score_Counter>().score);
				}
				GetComponent<AudioSource> ().clip = portal_open_sound;
				GetComponent<AudioSource> ().Play ();

                // ends the game 10 seconds after the vortex is active
                Invoke("endGame", 5);
			}
		}
		if (vortex) {
			for (int p =0; p < players.Length; p++) {
				players[p].transform.position = Vector2.MoveTowards (players [p].transform.position, transform.position, .25f); 
			}
		}
	}

	IEnumerator PortalAnimation(){
		while (true) {
			yield return new WaitForSeconds (0.2f);
			rendy.sprite = sprites [which_sprite];
			which_sprite++;
			if (which_sprite >= sprites.Count) {
				which_sprite = 1;
			}
		}
	}

    void endGame()
    {

        SceneManager.LoadScene("end_scene");
    }
}
