using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

	private Animator anim;
	private int inventory = 0;
	private int id;
	private int active_id;
	private GameObject Canvas;
	private GameObject ChestInteractionText;
	public bool chestInrange;
	private GameObject chestgobj;
	private GameObject Gun;
	private string currentweapon;
	PlayerMovement playerMovement;
	PlayerShooting playerShooting;
	PlayerWeaponSelection playerWeaponSelection;
	Chest chest;

	// Use this for initialization

	void Awake(){
		playerMovement = GetComponent<PlayerMovement> ();
		playerShooting = GetComponentInChildren<PlayerShooting> ();
		playerWeaponSelection = GetComponent<PlayerWeaponSelection> ();

	}

	void Start () {
		
		anim = GetComponent<Animator>();
		Canvas = GameObject.Find ("MainCanvas");
		ChestInteractionText = Canvas.transform.GetChild(6).gameObject;
		ChestInteractionText.SetActive (false);
		chestInrange = false;
		Gun = GameObject.FindGameObjectWithTag ("Gun");

		active_id = playerWeaponSelection.id;
		id = playerWeaponSelection.id;
		setCurrentWeaponTag();
		setWeaponAnimation ();
	}
	
	// Update is called once per frame
	void Update () {
		SetAnimation ();

		id = playerWeaponSelection.id;
		if (active_id != id) {
			setCurrentWeaponTag ();
			setWeaponAnimation ();
			active_id = id;
		}

	}

	// Triggers
	void OnTriggerEnter( Collider other){
		if (other.tag == "Chest"){
			ChestInteractionText.SetActive (true);
			chestInrange = true;
			chestgobj = other.gameObject;
		}
	}

	void OnTriggerExit( Collider other){
		if (other.tag == "Chest"){
			ChestInteractionText.SetActive (false);
			chestInrange = false;
			chestgobj.GetComponent<Chest> ().close (chestgobj);
		}
	}

	void SetAnimation () {
		if (Input.GetKeyDown (KeyCode.W)) {
			anim.SetBool ("AnimWalk", true);
			anim.SetBool ("AnimForward", true);
			anim.SetBool ("AnimRight", false);
			anim.SetBool ("AnimLeft", false);
			anim.SetBool ("AnimBack", false);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			if (chestInrange == true) {
				chestgobj.GetComponent<Chest> ().interactChest (chestgobj);

				if (playerMovement.enabled == true) {
					anim.SetBool ("AnimOpenBox", true);
					anim.SetBool ("AnimCloseBox", false);
					playerMovement.enabled = false;
					Gun.SetActive(false);
				} else {
					playerMovement.enabled = true;
					anim.SetBool ("AnimCloseBox", true);
					Gun.SetActive(true);
				}					
				if (playerShooting.enabled == false) {
					playerShooting.enabled = true;
				} else {
					playerShooting.enabled = false;
				}
			}
		} else {
			anim.SetBool ("AnimOpenBox", false);
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
		

		
	void setWeaponAnimation (){
		setAnimationsFalse ();
		if (currentweapon == "SMG" || currentweapon == "M4") {
			anim.SetBool ("AnimHasGun", true);
        }
        if (currentweapon == "Spear" || currentweapon == "Halbert")
        {
            anim.SetBool("AnimHasSpear", true);
        }
    }

	void setAnimationsFalse (){
		anim.SetBool ("AnimHasGun", false);
        anim.SetBool("AnimHasSpear", false);
    }	

	void setCurrentWeaponTag(){
		currentweapon = "None";
		if (id <= playerWeaponSelection.weapons.Count && id != -1) {
			currentweapon = playerWeaponSelection.weapons [id].tag;
			active_id = id;
		}
	}
}