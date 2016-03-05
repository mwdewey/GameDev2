using UnityEngine;
using System.Collections;

public class ReceiveKnockback : MonoBehaviour {
	/*This is a very simple script to put on whatever
	 * GameObject you'd like to be able to receive knockback. 
	*/

	public void Start(){
	}

	public void Update(){
	}

	public void GetKnockedBack(Vector2 knockback){
		//GetComponent<Rigidbody2D> ().AddForce (knockback);
		Vector3 delta = new Vector3();
		delta.x = transform.position.x + knockback.x;
		delta.y = transform.position.y + knockback.y;
		delta.z = 0;
		transform.position = delta;
		print ("knocked back! "+transform.position);
		//Whatever you do, make sure your walls are bigger than the amount of 
		//knockback defined in CauseKnockback, or a player may clip through 
		//walls when shot.
	}
}
