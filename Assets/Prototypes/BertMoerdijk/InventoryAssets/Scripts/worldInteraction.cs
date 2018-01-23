using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldInteraction : MonoBehaviour
{

    //
    private bool furnace_unlocked;
    private bool factory_unlocked;
    private bool printer_unlocked;
    private bool alien_machine_unlocked;

    private GameObject ChestPanel;
    private GameObject Canvas;
    private GameObject ActiveChest;
    private Inventory inv;
    private Crafting craftingWindows;
    public string inRangeOf;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        Canvas = GameObject.Find("MainCanvas");
        ChestPanel = Canvas.transform.GetChild(1).GetChild(0).gameObject;
        craftingWindows = inv.GetComponent<Crafting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            GetInteraction();
        }
    }

    // This sends out a ray from the camera to the pointer until it potentially hits a gameObject and returns which object it hit.
    void GetInteraction()
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;

        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            GameObject interactedObject = interactionInfo.collider.gameObject;
            if (interactedObject.tag == "1000")
            {
                Destroy(interactedObject);
                inv.AddItem(1000, 1);
            }
            if (interactedObject.tag == "1001")
            {
                Destroy(interactedObject);
                inv.AddItem(1001, 1);
            }
            if (interactedObject.tag == "1002")
            {
                Destroy(interactedObject);
                inv.AddItem(1002, 1);
            }
            if (interactedObject.tag == "1003")
            {
                Destroy(interactedObject);
                inv.AddItem(1003, 1);
            }
            if (interactedObject.tag == "1004")
            {
                Destroy(interactedObject);
                inv.AddItem(1004, 1);
            }
            if (interactedObject.tag == "1005")
            {
                Destroy(interactedObject);
                inv.AddItem(1005, 1);
            }
            if (interactedObject.tag == "1006")
            {
                Destroy(interactedObject);
                inv.AddItem(1006, 1);
            }
            if (interactedObject.tag == "1007")
            {
                Destroy(interactedObject);
                inv.AddItem(1007, 1);
            }
            if (interactedObject.tag == "1008")
            {
                Destroy(interactedObject);
                inv.AddItem(1008, 1);
            }
            if (interactedObject.tag == "1009")
            {
                Destroy(interactedObject);
                inv.AddItem(1009, 1);
            }
            if (interactedObject.tag == "1010")
            {
                Destroy(interactedObject);
                inv.AddItem(1010, 1);
            }
            if (interactedObject.tag == "WorkBench")
            {
                craftingWindows.ActivateWorkBench();
            }
            if (interactedObject.tag == "Furnace")
            {
                craftingWindows.ActivateFurnaceAndAnvil();
            }
            if (interactedObject.tag == "FactoryBelt")
            {
                craftingWindows.ActivateFactoryChainBelt();
            }
            if (interactedObject.tag == "3dprinter")
            {
                craftingWindows.ActivateThreeDPrinter();
            }
            if (interactedObject.tag == "alien_machine")
            {
                craftingWindows.ActivateWeirdAlienMachine();
            }
            if (interactedObject.tag == "Chest")
            {
                if (ChestPanel.activeSelf == true)
                {
                    inv.SaveChest(ActiveChest);
                    ChestPanel.gameObject.SetActive(false);
                }
                else
                {
                    ActiveChest = interactedObject;
                    ChestPanel.gameObject.SetActive(true);
                    inv.InitializeChest(interactedObject);
                }
            }
        }
    }
}
