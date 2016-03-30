using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fountain : MonoBehaviour {

	[System.Serializable] //Serializing = "Show this in the editor"
	public class FountainObject {
		public GameObject item;
		[Range (0,1)]
		public float ChanceOfAppearanceInFrame; //0 to 1 value of how frequent it ought to be
	}

	public List<FountainObject> items;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		foreach (FountainObject obj in items) {
			float roll_for_success = Random.Range (0f, 1f);
			if (roll_for_success < obj.ChanceOfAppearanceInFrame){
				int pos = Random.Range (0, 8);
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
				if (produced.tag == "Coin") {
					
				}
			}
		}
	}
}
