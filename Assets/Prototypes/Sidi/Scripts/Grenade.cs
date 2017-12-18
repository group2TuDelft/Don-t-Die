using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

	public float delay = 3f;
	public float damageRadius = 5f;
	public float blastForce = 800;
	public GameObject explosionParticle;

	float countdown;
	bool hasExploded = false;

	// Use this for initialization
	void Start () {
		countdown = delay;
	}
	
	// Update is called once per frame
	void Update () {
		countdown -= Time.deltaTime;

		if (countdown <= 0f && !hasExploded) {
			Explode ();
			hasExploded = true;
		}
	}

	void Explode(){

		Debug.Log ("I am back");
		GameObject obj = Instantiate (explosionParticle, transform.position, transform.rotation);

		Collider[] colliders = Physics.OverlapSphere (transform.position, damageRadius);

		foreach(Collider nearbyObject in colliders){
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody> ();

			if (rb != null) {
				rb.AddExplosionForce (blastForce, transform.position, blastForce);
			}
		}
		Destroy (this.gameObject);
		Destroy (obj, 3f);
	}
}
