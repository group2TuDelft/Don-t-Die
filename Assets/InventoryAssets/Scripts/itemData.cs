using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
using UnityEngine.EventSystems;

public class itemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public Item item;
    public int amount;
    public int slotid;
    
    private Inventory inv;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    // Drag an drop functions : 

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            // rendering order shizzle
            // Sets position of the item to the mouse pointer
            this.transform.SetParent(this.transform.parent.parent.parent.parent); 
            this.transform.position = eventData.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

    }

    //
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            // Sets position of the item to the mouse pointer
            this.transform.position = eventData.position;
        }
    }

    //
    public void OnEndDrag(PointerEventData eventData)
    {
        // Set the item to the right slot position, either back to its old one or to the new one it found.
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.transform.SetParent(inv.slots[slotid].transform);
        this.transform.position = inv.slots[slotid].transform.position;
    }
}
