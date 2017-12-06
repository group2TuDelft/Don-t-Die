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
    [SerializeField] GameObject Slider;
    private int activeSlotId = 0;
    private int activeWeapon = -1;

    IEnumerator Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        yield return new WaitForSeconds(1/10);
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
        activeWeapon = inv.items[activeSlotId].ID;

        Debug.Log(activeWeapon);

        // Visual
        Slider.transform.position  = inv.slots[activeSlotId].transform.position;
    }

    public int GetWeaponID ()
    {
        return activeWeapon;
    }

}
