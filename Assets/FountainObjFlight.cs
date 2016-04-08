using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FountainObjFlight : MonoBehaviour {

	List<GameObject> moving_objs = new List<GameObject> ();
	List<GameObject> remove_from_moving_objs = new List<GameObject> ();
	float ground;
	private GameObject spawn_object;

	// Use this for initialization
	void Start () {
		ground = transform.position.y - 1;
	}

	// Update is called once per frame
	void Update () {
		if (spawn_object!=null) {
			//print ("oh yeah");
			Vector2 loc = new Vector2 (transform.position.x + Random.Range (-.6f, .6f), transform.position.y+.5f);
			GameObject new_obj = (GameObject) Instantiate(spawn_object, loc, Quaternion.identity);
			///new_coin.tag = "Untagged";
			Rigidbody2D rb;
			if (new_obj.GetComponent<Rigidbody2D> () == null)
				rb = new_obj.AddComponent <Rigidbody2D>();
			else
				rb = new_obj.GetComponent<Rigidbody2D> ();
			rb.velocity = new Vector2 (Random.Range (-.75f, .75f), 6);
			moving_objs.Add (new_obj);
			spawn_object = null;
		}

		foreach (GameObject obj in moving_objs) {
			Rigidbody2D r = obj.GetComponent<Rigidbody2D> ();
			r.velocity = new Vector2(r.velocity.x, r.velocity.y - 9.8f * Time.deltaTime);
			if (Vector2.Distance(obj.transform.position, transform.position)> (2+Random.Range(-.5f,.5f)) && obj.transform.position.y<transform.position.y-.8f){     //obj.transform.position.y < ground) {
				remove_from_moving_objs.Add (obj);
                r.velocity = Vector2.zero;
				//Destroy (r); //remove the rigidbody
				///coin.tag = "Coin";
				//now it's just a normal coin
			}
		}


		//weird stuff:*
		foreach (GameObject g in remove_from_moving_objs) {
			moving_objs.Remove (g);
		}
		remove_from_moving_objs.Clear ();
		// *why did I do this? It's bad practice to remove objects 
		//from something you're iterating through, so I save the 
		//names of my targets and remove them afterward

	}

	public void SpawnObject(GameObject obj){
		spawn_object = obj;
	}
}
