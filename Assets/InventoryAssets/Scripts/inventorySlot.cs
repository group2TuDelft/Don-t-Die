using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
using UnityEngine.EventSystems;

// Script takes care of the switching in the drag and drop system
public class inventorySlot : MonoBehaviour, IDropHandler {

    public int id;
    private Inventory inv;
    
    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Get all item data
        itemData droppedItem = eventData.pointerDrag.GetComponent<itemData>();

        // Takes care of the case of the item being dropped on an empty slot.
        if (inv.items[id].ID == -1)
        {
            // delete old slot ref.
            inv.items[droppedItem.slotid] = new Item();
            // insert new slot ref.
            inv.items[id] = droppedItem.item;
            // move item to new slot.
            droppedItem.slotid = id;
        }
        // Takes care of the case of there already being an item in the inventory slot.
        else
        {
            if (this.transform.GetChild(0) != null) { 
            // Get Item to be replaced to the old location of the dropped object
            Transform item = this.transform.GetChild(0);                              // Get old item
            item.GetComponent<itemData>().slotid = droppedItem.slotid;                // Set slot id to new id
            item.transform.SetParent(inv.slots[droppedItem.slotid].transform);             // set new parent
            item.transform.position = inv.slots[droppedItem.slotid].transform.position;    // set new position

            // Update the slot ids within the inventory itself.
            inv.items[droppedItem.slotid] = item.GetComponent<itemData>().item;
            inv.items[id] = droppedItem.item;
            
            // Get dropped object to its new location
            droppedItem.slotid = id;
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;
            }
        }
    }

}
