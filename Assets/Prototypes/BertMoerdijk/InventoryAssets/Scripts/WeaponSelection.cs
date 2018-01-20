using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This function will be responsible for the selection of weapons in the first 4 slots.
// It will handle:
// - A visual clue as to which weapon is selected.
// - A function to ask for which weapon is selected -> outputs id
// - Make it possible to toggle between the weapons with key 1-4. (might be in worldinteraction?)

public class WeaponSelection : MonoBehaviour {

    private Inventory inv;
    // visable slider located over the slots
    private GameObject Slider;
    private int activeSlotId = 0;
    private GameObject Canvas;

    void Start ()
    {
        Canvas = GameObject.Find("MainCanvas");
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        Slider = Canvas.transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
        Slider.transform.position = inv.slots[activeSlotId].transform.position;
        StartCoroutine("WaitAndTransform");
    }
    
    // This function is needed in order to transform to the first slot panel at the start of the game
    // This is because the position of the slot isnt defined in time otherwise. 
    IEnumerator WaitAndTransform()
    {
        yield return new WaitForSeconds(1/10f);
        Slider.transform.position = inv.slots[activeSlotId].transform.position;
    }
    
    // Registers keys 1 - 4.
    void Update ()
    {
        if (Input.GetKeyDown("1"))
        {
            UpdateSelectedWeapon(1);
        }
        if (Input.GetKeyDown("2"))
        {
            UpdateSelectedWeapon(2);
        }
        if (Input.GetKeyDown("3"))
        {
            UpdateSelectedWeapon(3);
        }
        if (Input.GetKeyDown("4"))
        {
            UpdateSelectedWeapon(4);
        }
    }

    private void UpdateSelectedWeapon (int key)
    {
        // Behind the scenes
        activeSlotId = key - 1;

        // Visual
        Slider.transform.position  = inv.slots[activeSlotId].transform.position;
        Slider.transform.localScale = Canvas.transform.GetChild(0).GetChild(0).GetChild(0).localScale;
    }

    public int GetWeaponID ()
    {
        return inv.items[activeSlotId].ID;
    }
		
	public string GetWeaponName(){
		return inv.items [activeSlotId].Title;
	}

}
