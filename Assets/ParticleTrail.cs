using UnityEngine;
using System.Collections;

public class ParticleTrail : MonoBehaviour { 
	/*Just add this script to any item that needs a 
	 * particle trail, assign the particle system, 
	 * and call addParticleTrails in the item's 
	 * Activate function.
	 * GetComponent<ParticleTrail>().addParticleTrails(holder);
	*/

	public ParticleSystem particle_system;

	public void addParticleTrails(GameObject holder){
		ParticleSystem ps = (ParticleSystem) Instantiate (particle_system, transform.position, Quaternion.identity);
		GetComponent<Item>().my_ps = ps;
		//ps.transform.parent = gameObject.transform;
		ps.startColor = (Color) holder.GetComponent<PlayerController> ().playerColor;

		//ps.simulationSpace = ParticleSystemSimulationSpace.World;
		//ps.GetComponent<ParticleRenderer> ().sortingOrder = 10;
		//ps.GetComponent<ParticleEmitter> ().maxEmission = 100;
	}

	void Update(){
		if (GetComponent<Item>().my_ps != null) {
			GetComponent<Item>().my_ps.transform.position = gameObject.transform.position;
		}
	}
}

