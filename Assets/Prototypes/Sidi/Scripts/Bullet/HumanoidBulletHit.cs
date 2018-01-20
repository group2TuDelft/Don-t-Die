using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidBulletHit : MonoBehaviour {

	public GameObject hitParticle;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter( Collider other){
		if (other.tag == "Player") {
			PlayerHealth playerHealth = other.GetComponent<PlayerHealth> ();
			playerHealth.TakeDamage (30,"Humanoid");
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Player")
		{
			Explode ();
		}
	}


	void Explode (){
		GameObject obj = Instantiate (hitParticle, transform.position, transform.rotation);
		Destroy (obj, 1.5f);
		Destroy (this.gameObject);
	}
}
