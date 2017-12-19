using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

	private Animator anim;
	private CharacterController controller;
	private int inventory = 0;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		controller = GetComponent <CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		SetAnimation ();
	}

	void SetAnimation () {
		if (Input.GetKeyDown (KeyCode.W)) {
			anim.SetBool ("AnimWalk", true);
			anim.SetBool ("AnimForward", true);
			anim.SetBool ("AnimRight", false);
			anim.SetBool ("AnimLeft", false);
			anim.SetBool ("AnimBack", false);
		}
		if (Input.GetKeyUp (KeyCode.W)) {
			anim.SetBool ("AnimForward", false);
			anim.SetBool ("AnimWalk", false);
						
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			anim.SetBool ("AnimWalk", true);
			anim.SetBool ("AnimForward", false);
			anim.SetBool ("AnimRight", false);
			anim.SetBool ("AnimLeft", false);
			anim.SetBool ("AnimBack", true);
		}
		if (Input.GetKeyUp (KeyCode.S)) {
			anim.SetBool ("AnimBack", false);
			anim.SetBool ("AnimWalk", false);
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			anim.SetBool ("AnimWalk", true);
			anim.SetBool ("AnimForward", false);
			anim.SetBool ("AnimRight", false);
			anim.SetBool ("AnimLeft", true);
			anim.SetBool ("AnimBack", false);
		}
		if (Input.GetKeyUp (KeyCode.A)) {
			anim.SetBool ("AnimWalk", false);
			anim.SetBool ("AnimLeft", false);
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			anim.SetBool ("AnimWalk", true);
			anim.SetBool ("AnimForward", false);
			anim.SetBool ("AnimRight", true);
			anim.SetBool ("AnimLeft", false);
			anim.SetBool ("AnimBack", false);
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			anim.SetBool ("AnimWalk", false);
			anim.SetBool ("AnimRight", false);
		}
			
		
		if (Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.W)) {
			anim.SetBool ("AnimRun", true);
		} else {
			anim.SetBool ("AnimRun", false);
		} 
		
		if (Input.GetKeyDown (KeyCode.Z)) {
			SetWeapon ();
			if (inventory == 0) {
				anim.SetBool ("AnimHasGun", false);
				anim.SetBool ("AnimHasSpear", false);
			} 
			if (inventory == 1) {
				anim.SetBool ("AnimHasGun", true);
				anim.SetBool ("AnimHasSpear", false);
			} 
			if (inventory == 2) {
				anim.SetBool ("AnimHasGun", false);
				anim.SetBool ("AnimHasSpear", true);
			} 
		}
		if (Input.GetKey (KeyCode.Mouse0)) {
			anim.SetBool ("AnimShooting", true);
		} else {
			anim.SetBool ("AnimShooting", false);
		} 
		if (Input.GetKey (KeyCode.Mouse1)) {
			anim.SetBool ("AnimAiming", true);
		if (Input.GetKeyUp (KeyCode.Mouse1)) {
			anim.SetBool ("AnimThrow", true);
			anim.SetBool ("AnimAiming", false);
			}
		} else {
			anim.SetBool ("AnimAiming", false);
		}			
	}

	void SetWeapon () {
		if (inventory != 2) {
			inventory += 1;
		} else {
			inventory = 0;
		}
	}
			
}