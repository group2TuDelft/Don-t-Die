using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script will be used to couple the inventory database to the UI and make it visable in game.
public class Inventory : MonoBehaviour {

    // Required UI objects
    GameObject inventoryPanel;
    GameObject slotPanel;

    // Temporary UI elements:
    // Chest objects/vars/int etc
    public GameObject ChestPanel;
    private int chestSlotAmount;

    ItemDatabase database;

    // Universal inventory item and slots
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    private int slotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    // The start function will intiate the inventory slots.
    void Start()
    {
        // Grabs the itemdatabase functions
        database = GetComponent<ItemDatabase>();
        //
        slotAmount = 10;
        inventoryPanel = GameObject.Find("InventoryPanel");
        slotPanel = inventoryPanel.transform.Find("SlotPanel").gameObject;
        // Initializes the inventory Slots
        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<inventorySlot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }

        // 
        InitializeChest();

        // Adding Initial Items.
        AddItem(0);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
    }

    // function used to add an item
    public void AddItem(int id)
    {
        Item itemToAdd = database.FetchItemByID(id);

        if (itemToAdd.Stackable && CheckItemInInventory(itemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == id)
                {
                    itemData data = slots[i].transform.GetChild(0).GetComponent<itemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                }
            }
        }
        else {
            for (int i = 0; i < items.Count; i++)
            {
                // Checks if the slot is empty. (in the start function all slots are initialized with id = -1)
                if (items[i].ID == -1)
                {
                    items[i] = itemToAdd;
                    //
                    GameObject itemObj = Instantiate(inventoryItem);

                    //  Set the item in itemData to be the added item.
                    itemObj.GetComponent<itemData>().item = itemToAdd;

                    // Store the slot id in the item.
                    itemObj.GetComponent<itemData>().slotid = i;

                    // Parent the item to the inventory slot.
                    itemObj.transform.SetParent(slots[i].transform);

                    // Sets the object in the centre of the item slot.
                    itemObj.transform.position = slots[i].transform.position;

                    // Sets the corrosponding sprite to the slot.
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;

                    itemObj.name = itemToAdd.Title;

                    break;
                }
            }
        }

    }

    // Function that checks if an item is in the inventory.
    bool CheckItemInInventory(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == item.ID) {
                return true;
            }
        }
        return false;
    }

    public void InitializeChest() {
    // Chest Initialization: //////////////////////////////////////////////

    chestSlotAmount = 8;
        // Chest Slot Initialization:
        for (int j = 0; j<chestSlotAmount; j++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[j + slotAmount].GetComponent<inventorySlot>().id = j + slotAmount;
            slots[j + slotAmount].transform.SetParent(ChestPanel.transform);
        }

        ////////////////////////////////////////////////////////////////////////
    }
}
