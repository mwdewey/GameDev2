using UnityEngine;
using System.Collections;

public class Vegano_Ultimate : MonoBehaviour {

	GameObject minion;
	Transform t;

	// Use this for initialization
	void Start () {
		minion = (GameObject) Resources.Load ("Prefabs/Enemy");
		t = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ultimate () {
		Debug.Log ("I AM DOING MY ULTIMATE ATTACK NOW");
		for (int x = 0; x < 3; x++){
			GameObject temp = Instantiate (minion);
			temp.transform.position = new Vector3 (t.position.x + 1 * Mathf.Cos (x * 2 * Mathf.PI / 3), t.position.y + 1 * Mathf.Sin (x * 2 * Mathf.PI / 3), t.position.x);
			temp.GetComponent<EnemyBehavior> ().master = t;
		}
	}
}
