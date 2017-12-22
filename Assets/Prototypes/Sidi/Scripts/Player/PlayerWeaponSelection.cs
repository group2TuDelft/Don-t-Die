using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSelection : MonoBehaviour {


	WeaponSelection selector;
	int id;
	int active_id;
	GameObject inv;
	public List<GameObject> weapons = new List<GameObject>();

	// Use this for initialization
	void Start () {
		weapons.Add(GameObject.FindGameObjectWithTag ("Gun"));
		inv = GameObject.Find ("Inventory");
		selector = inv.GetComponent<WeaponSelection>();
		active_id = selector.GetWeaponID ();
	}
	
	// Update is called once per frame
	void Update () {

		id = selector.GetWeaponID ();
		Debug.Log (id);

		if (active_id != id) {
			changeWeapon ();
		}
	
	}

	private void changeWeapon ()
	{

	
		if (id <= weapons.Count && id != -1) {
			Debug.Log ("yo");
			weapons [id].SetActive (true);
			if (active_id <= weapons.Count && active_id != -1) {
				weapons [active_id].SetActive (false);
			}
			active_id = id;
		} else {
			if (active_id <= weapons.Count && active_id != -1) {
				weapons [active_id].SetActive (false);
			}
			active_id = id;
		}
	}
}
