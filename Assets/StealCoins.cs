using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StealCoins : MonoBehaviour {

	public GameObject holder;
	int number_to_pilfer = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "PlayerObject") { 
			//we found a victim!

			//how many coins can you steal, you slimey theif?
			int their_holdings = other.gameObject.GetComponent<Score_Counter> ().score;
			if (their_holdings < number_to_pilfer) {
				number_to_pilfer = their_holdings;
			}

			//steal it
			other.gameObject.GetComponent<Score_Counter> ().score -= number_to_pilfer;
			other.gameObject.transform.Find ("Player 1 UI").transform.Find ("Coin Text").gameObject.GetComponent<Text> ().text = other.gameObject.GetComponent<Score_Counter> ().score.ToString ();

			//update your holdings
			holder.GetComponent<Score_Counter> ().score += number_to_pilfer;
			holder.transform.Find ("Player 1 UI").transform.Find ("Coin Text").gameObject.GetComponent<Text> ().text = holder.GetComponent<Score_Counter> ().score.ToString ();

		}
	}
}
