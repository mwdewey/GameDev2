using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score_Counter : MonoBehaviour {

	public int score;
	Transform t;
	public bool in_portal;
	float portal_progress;
	private SpriteRenderer pp_sprite;
	Color pp_color;
    Text score_label;

	// Use this for initialization
	void Start () {
		score = 0;
		t = GetComponent<Transform> ();
		in_portal = false;
        pp_sprite = transform.Find("Portal Progress").gameObject.GetComponent<SpriteRenderer>();
		pp_color = pp_sprite.color;
        score_label = transform.Find("Player 1 UI").transform.Find("Coin Text").gameObject.GetComponent<Text>();
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Coin" && !GetComponent<PlayerController>().unconscious){
            other.GetComponent<Rigidbody2D>().velocity = Vector3.Scale((t.position - other.transform.position).normalized, new Vector3(3f, 3f, 0));

			if (Vector3.Distance(t.position, other.transform.position) <= 0.5 && other.GetComponent<Renderer>().enabled){
                score += 1;
				string pid = GetComponent<PlayerController> ().PID;
				transform.Find ("Player 1 UI").transform.Find("Coin Text").gameObject.GetComponent<Text>().text = score.ToString();
                other.GetComponent<Renderer>().enabled = false;
                Destroy(other);
                Manager.coins_remaining -= 1;
            }
        }
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Coin") {
			other.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		}
	}

	public void progress_portal(){
		if (in_portal) {
			portal_progress += 0.5f;
		}
	}
	
	// Update is called once per frame
	void Update () {
        score_label.text = score.ToString();
		if (Input.GetKey (KeyCode.Space)) {
			Debug.Log ("Your score: " + score);
		}
		if (in_portal) {
			portal_progress -= Time.deltaTime;
			if (portal_progress < 0) {
				portal_progress = 0;
			}
			pp_sprite.color = new Color (pp_color.r, pp_color.g, pp_color.b, portal_progress / 5f);
			if (portal_progress >= 5) {
				GetComponent<SpriteRenderer> ().enabled = false;
				Destroy (gameObject);
				pp_sprite.enabled = false;
				Destroy (pp_sprite.gameObject);
				Debug.Log ("Player has escaped with " + score + " coins!");
                PlayerPrefs.SetInt("p" + GetComponent<PlayerController>().PID + "score", score);
			}
		} 
		else if (pp_sprite.color.a > 0f) {
			portal_progress -= Time.deltaTime;
			if (portal_progress < 0) {
				portal_progress = 0;
			}
			pp_sprite.color = new Color (pp_color.r, pp_color.g, pp_color.b, portal_progress / 5f);
		}
	}
}
