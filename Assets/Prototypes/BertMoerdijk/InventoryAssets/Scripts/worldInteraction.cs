using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldInteraction : MonoBehaviour
{

    //

    [SerializeField] GameObject ChestPanel;
    private GameObject ActiveChest;
    private Inventory inv;
    private Crafting craftingWindows;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
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
            if (interactedObject.tag == "Red Box")
            {
                interactedObject.SetActive(false);
                inv.AddItem(2);
            }
            if (interactedObject.tag == "WorkBench")
            {
                craftingWindows.ActivateWorkBench();
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
