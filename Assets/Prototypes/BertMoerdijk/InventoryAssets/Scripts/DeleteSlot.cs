using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
using UnityEngine.EventSystems;

// Script takes care of the switching in the drag and drop system
public class DeleteSlot : MonoBehaviour, IDropHandler
{
    
    private Inventory inv;
    private int itemID;
    private int amount;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {

        itemData droppedItem = eventData.pointerDrag.GetComponent<itemData>();
        itemID = inv.items[droppedItem.slotid].ID;
        amount = droppedItem.amount;
        StartCoroutine(WaitABitAndDelete());

     }

    IEnumerator WaitABitAndDelete()
    {
        yield return new WaitForSeconds(0.01f);
        inv.DeleteItem(itemID, amount);
    }

}
