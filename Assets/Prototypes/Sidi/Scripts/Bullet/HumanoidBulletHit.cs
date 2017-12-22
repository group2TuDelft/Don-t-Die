using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidBulletHit : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter( Collider other){
		if (other.tag == "Player") {
			PlayerHealth playerHealth = other.GetComponent<PlayerHealth> ();
			playerHealth.TakeDamage (30);
			this.GetComponent<Renderer> ().enabled = false;
		}
	}
}
