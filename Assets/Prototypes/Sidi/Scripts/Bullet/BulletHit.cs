using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour {

	public GameObject hitParticle;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter( Collider other){
        
		if (other.tag == "Enemy")
        {
			other.GetComponent<ColliderNameFinder> ().scriptname( other.gameObject );
		}

	}

	void OnCollisionEnter(Collision other){

			Explode ();
	}

	void Explode (){
		GameObject obj = Instantiate (hitParticle, transform.position, transform.rotation);
		Destroy (obj, 1.5f);
		Destroy (this.gameObject);
	}
}
