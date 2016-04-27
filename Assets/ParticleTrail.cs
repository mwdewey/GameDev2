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
	bool active;

	public ParticleSystem particle_system;

	public void Activate(GameObject holder){
		ps = (ParticleSystem) Instantiate (particle_system, transform.position, Quaternion.identity);
		t = ps.gameObject.GetComponent<Transform> ();
		t2 = GetComponent<Transform> ();
		//ps.transform.parent = gameObject.transform;
		ps.startColor = (Color) holder.GetComponent<PlayerController> ().playerColor;
		active = true;

		//ps.simulationSpace = ParticleSystemSimulationSpace.World;
		//ps.GetComponent<ParticleRenderer> ().sortingOrder = 10;
		//ps.GetComponent<ParticleEmitter> ().maxEmission = 100;
	}

	public void Deactivate(){
		ParticleSystem.EmissionModule em = ps.emission;
		em.enabled = false;
	}

	public void Reactivate(){
		ParticleSystem.EmissionModule em = ps.emission;
		em.enabled = true;
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

