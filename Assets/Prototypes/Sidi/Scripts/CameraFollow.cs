using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
		
	public float smoothing = 5f;
	//public Transform target;
	Vector3 offset;

	void Start () {
		
		offset = transform.position - GameObject.FindWithTag("Player").transform.position;
	}
	

	void FixedUpdate () {
		Vector3 targetPos = GameObject.FindWithTag("Player").transform.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetPos, smoothing * Time.deltaTime);
	}
}
