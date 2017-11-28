using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldInteraction : MonoBehaviour
{

    //
    public GameObject ChestPanel;

    private Inventory inv;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        // Left Click Interactions
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            GetInteraction();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetEscInteraction();
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
                Debug.Log ("Picked Up Red Box");
            }
            if (interactedObject.tag == "White Cilinder")
            {
                interactedObject.SetActive(false);
                inv.AddItem(3);
            }
            if (interactedObject.tag == "Chest")
            {
                Debug.Log("Clicked Chest");

                if (ChestPanel.activeSelf == true)
                {
                    ChestPanel.gameObject.SetActive(false);
                }
                else
                {
                    ChestPanel.gameObject.SetActive(true);
                }

            }
        }
    }

    private void GetEscInteraction()
    {
        if (ChestPanel.activeSelf == true)
        {
            ChestPanel.gameObject.SetActive(false);
        }
    }

}
