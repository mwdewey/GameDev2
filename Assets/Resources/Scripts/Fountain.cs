using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fountain : MonoBehaviour {

	[System.Serializable] //Serializing = "Show this in the editor"
	public class FountainObject {
		public GameObject item;
		public int max_instances;//make this negative for infinite items
		[Range (0,.1f)]
		public float ChanceOfAppearanceInFrame; //0 to 1 value of how frequent it ought to be
	}

	public List<FountainObject> items;
	List<FountainObject> removeFromItems = new List<FountainObject>();

	// Use this for initialization
	void Start () {
		}
	
	// Update is called once per frame
	void Update () {
		foreach (FountainObject obj in items) {
			float roll_for_success = Random.Range (0f, 1f);
			if (roll_for_success < obj.ChanceOfAppearanceInFrame){
				GetComponent<FountainObjFlight> ().SpawnObject (obj.item);

				/*int pos = Random.Range (0, 8);
				int x_offset = 0;
				int y_offset = 0;
				switch (pos) {
				case 0:
					x_offset = -1;
					y_offset = 1;
					break;
				case 1:
					x_offset = 0;
					y_offset = 1;
					break;
				case 2:
					x_offset = 1;
					y_offset = 1;
					break;
				case 3:
					x_offset = 1;
					y_offset = 0;
					break;
				case 4:
					x_offset = 1;
					y_offset = -1;
					break;
				case 5:
					x_offset = 0;
					y_offset = -1;
					break;
				case 6:
					x_offset = -1;
					y_offset = -1;
					break;
				case 7:
					x_offset = -1;
					y_offset = 0;
					break;
				}
				float x_rand_modifier = Random.Range (-.4f, .4f);
				float y_rand_modifier = Random.Range (-.4f, .4f);
				GameObject produced = (GameObject)Instantiate (obj.item, new Vector2 (transform.position.x+x_offset+x_rand_modifier, transform.position.y+y_offset+y_rand_modifier), Quaternion.identity);
				*/

				if (obj.item.tag == "Coin")	Manager.coins_remaining += 1;

				obj.max_instances--;
				if (obj.max_instances <= 0) {
					//print ("spawned all the " + obj.item + "s");
					removeFromItems.Add (obj);
				}
				if (items.Count == 0) {
					gameObject.GetComponent<Animator> ().SetBool ("dead", true);
					//print ("Fountain: *cough*");
					return;
				}
			}
		}

		//why is this here?
		//It's bad practice to remove from a list as you're 
		//looping through 
		foreach (FountainObject o in removeFromItems) {
			items.Remove (o);
		}
		removeFromItems.Clear ();
	}
}
