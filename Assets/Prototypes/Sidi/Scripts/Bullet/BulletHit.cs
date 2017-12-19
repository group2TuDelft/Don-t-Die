using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter( Collider other){
		if (other.tag == "Enemy") {
			EnemyHealth enemyHealth = other.GetComponent<EnemyHealth> ();
			enemyHealth.TakeDamage (30);
			this.GetComponent<Renderer> ().enabled = false;
		}
	}
}
