using UnityEngine;
using System.Collections;

public class ParticleTrail : MonoBehaviour { 
	/*Just add this script to any item that needs a 
	 * particle trail, assign the particle system, 
	 * and call addParticleTrails in the item's 
	 * Activate function.
	 * GetComponent<ParticleTrail>().addParticleTrails(holder);
	*/

	ParticleSystem ps;
	Transform t;
	Transform t2;
	public bool active;

	public ParticleSystem particle_system;

	public void addParticleTrails(GameObject holder){
		ps = (ParticleSystem) Instantiate (particle_system, transform.position, Quaternion.identity);
		t = ps.gameObject.GetComponent<Transform> ();
		t2 = GetComponent<Transform> ();
		//ps.transform.parent = gameObject.transform;
		ps.startColor = (Color) holder.GetComponent<PlayerController> ().playerColor;

		//ps.simulationSpace = ParticleSystemSimulationSpace.World;
		//ps.GetComponent<ParticleRenderer> ().sortingOrder = 10;
		//ps.GetComponent<ParticleEmitter> ().maxEmission = 100;
	}

	void OnDestroy(){
		Destroy (ps);
	}

	void Update(){
		if (active) {
			t.position = t2.position;
		}
	}
}

