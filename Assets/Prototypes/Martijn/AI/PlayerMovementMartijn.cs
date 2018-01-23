using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementMartijn : MonoBehaviour {

    public Rigidbody rb;
    public float speed = 500f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKey("w"))
        {
            rb.AddForce(0, 0, speed * Time.deltaTime, ForceMode.VelocityChange);
        }
        if (Input.GetKey("s"))
        {
            rb.AddForce(0, 0, -speed * Time.deltaTime, ForceMode.VelocityChange);
        }
		if (Input.GetKey("a"))
        {
            rb.AddForce(-speed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey("d"))
        {
            rb.AddForce(speed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        //if (Input.GetKey("q"))
        
            //rb.AddTorque(0, speed * Time.deltaTime, 0, ForceMode.VelocityChange);
        
        //if (Input.GetKey("e"))
        
            //rb.AddTorque(0, -speed * Time.deltaTime, 0, ForceMode.VelocityChange);
        

    }
}
