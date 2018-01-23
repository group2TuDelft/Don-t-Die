using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSelection : MonoBehaviour {


	WeaponSelection selector;
	public int id;
	int active_id;
	GameObject inv;
	public List<GameObject> weapons = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        weapons.Add(new GameObject());                              // Rock
        weapons.Add(new GameObject());                              // 
        weapons.Add(GameObject.FindGameObjectWithTag("Halbert"));   // Halbert
        weapons.Add(GameObject.FindGameObjectWithTag("catapult"));  // Catapult
        weapons.Add(GameObject.FindGameObjectWithTag("Spear"));     // Spear
        weapons.Add(GameObject.FindGameObjectWithTag ("SMG"));      // SMG
        weapons.Add(GameObject.FindGameObjectWithTag("Bow"));       // Bow
        weapons.Add(GameObject.FindGameObjectWithTag("M4"));        // M4
        weapons.Add(GameObject.FindGameObjectWithTag("CrossBow"));  // Xbow
        weapons.Add(GameObject.FindGameObjectWithTag("BlunderBuss"));  // Blunderbuss
        weapons.Add(GameObject.FindGameObjectWithTag("m1911"));     // M1911
        weapons.Add(GameObject.FindGameObjectWithTag("revolver"));     // Revolver
        weapons.Add(GameObject.FindGameObjectWithTag("shotgun"));     // Shotgun
        weapons.Add(GameObject.FindGameObjectWithTag("scifihandgun"));     // scifihandgun
        weapons.Add(GameObject.FindGameObjectWithTag("scifibiggun"));     // scifihandgun
        weapons.Add(GameObject.FindGameObjectWithTag("axe"));     // scifihandgun

        inv = GameObject.Find ("Inventory");
		selector = inv.GetComponent<WeaponSelection>();
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive (false);
        }
		active_id = selector.GetWeaponID ();


	}
	
	// Update is called once per frame
	void Update () {

		id = selector.GetWeaponID ();

		if (active_id != id) {
			changeWeapon ();
		}
	
	}

	private void changeWeapon ()
	{

	
		if (id <= weapons.Count && id != -1) {
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
