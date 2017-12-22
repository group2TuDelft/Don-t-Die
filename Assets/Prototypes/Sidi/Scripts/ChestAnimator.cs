using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimator : MonoBehaviour {
	private Animator anim;
	private CharacterController controller;
	public GameObject player;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		controller = GetComponent <CharacterController>();
		anim.SetBool ("AnimClosed", true);

	}
	
	// Update is called once per frame
	void Update () {
		SetAnimation ();
	}



	void SetAnimation () {
		if (Input.GetKeyDown (KeyCode.E)) {
			if (player.GetComponent<PlayerAnimator> ().chestInrange == true) {
				if (anim.GetBool ("AnimClosed") == true) {
					anim.SetBool ("AnimOpen", true);
					anim.SetBool ("AnimClose", false);
					anim.SetBool ("AnimClosed", false);

				} else {
					anim.SetBool ("AnimOpen", false);
					anim.SetBool ("AnimClose", true);
					anim.SetBool ("AnimClosed", true);
				}
			}
		}
	}			
}
