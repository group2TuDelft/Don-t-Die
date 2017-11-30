using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectForceFollow: MonoBehaviour {
    public Rigidbody rbself;
    public Transform transplayer;
    public Transform transself;
    public float speed = 14f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {


        rbself.AddForce((transplayer.position - transself.position).normalized*speed, ForceMode.VelocityChange);
		// Vector speler - vector enemy is de vector die de richting aangeeft tussen speler en enemy
        // Als de enemy dit dus als richtingsvector heeft, zal die dus altijd richting de speler gaan
	}
}
