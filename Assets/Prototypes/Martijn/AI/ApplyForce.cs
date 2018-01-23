using UnityEngine;

public class TestBheavoir : MonoBehaviour {
    public Rigidbody rb;
    public int speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rb.AddForce(0, speed * Time.deltaTime, speed * 2 * Time.deltaTime);
		
	}
}
