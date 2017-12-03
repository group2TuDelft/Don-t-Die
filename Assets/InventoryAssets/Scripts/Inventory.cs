using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script will be used to couple the inventory database to the UI and make it visable in game.
public class Inventory : MonoBehaviour
{

    // Required UI objects
    GameObject inventoryPanel;
    GameObject WeaponSlotPanel;
    GameObject InventorySlotPanel;

    // Temporary UI elements:
    // Chest objects/vars/int etc
    public GameObject ChestPanel;

    ItemDatabase database;

    // Universal inventory item and slots
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    private int weaponSlotAmount;
    public int chestSlotAmount;
    private int inventorySlotAmount;

    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    // The start function will intiate the inventory slots.
    void Start()
    {
        // Grabs the itemdatabase functions
        database = GetComponent<ItemDatabase>();
        //
        weaponSlotAmount = 4;
        inventorySlotAmount = 8;
        //
        inventoryPanel = GameObject.Find("InventoryPanel");
        WeaponSlotPanel = inventoryPanel.transform.Find("WeaponSlotPanel").gameObject;
        InventorySlotPanel = inventoryPanel.transform.Find("InventorySlotPanel").gameObject;
        // Initializes the first 4 inventory Slots.
        for (int i = 0; i < weaponSlotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<inventorySlot>().id = i;
            slots[i].transform.SetParent(WeaponSlotPanel.transform);
        }
        // Initializes the next 8 inventory Slots.
        for (int j = 0; j < inventorySlotAmount; j++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[j + weaponSlotAmount].GetComponent<inventorySlot>().id = j + weaponSlotAmount;
            slots[j + weaponSlotAmount].transform.SetParent(InventorySlotPanel.transform);
        }

        // 
        //InitializeChest();

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
        else
        {
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
            if (items[i].ID == item.ID)
            {
                return true;
            }
        }
        return false;
    }

    public void InitializeChest(GameObject chest)
    {
        // Chest Initialization: //////////////////////////////////////////////

        chestSlotAmount = 8;
        
        // Initialize Slots
        for (int j = 0; j < chestSlotAmount; j++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[j + weaponSlotAmount + inventorySlotAmount].GetComponent<inventorySlot>().id = j + weaponSlotAmount + inventorySlotAmount;
            slots[j + weaponSlotAmount + inventorySlotAmount].transform.SetParent(ChestPanel.transform);
            items[j + weaponSlotAmount + inventorySlotAmount] = chest.GetComponent<ChestStorage>().itemsInChest[j];

            if (items[j + weaponSlotAmount + inventorySlotAmount].ID != -1) { 

            // Draw Items
            GameObject itemObj = Instantiate(inventoryItem);
            itemObj.GetComponent<itemData>().item = items[j + weaponSlotAmount + inventorySlotAmount];
            itemObj.GetComponent<itemData>().slotid = j + weaponSlotAmount + inventorySlotAmount;
            itemObj.transform.SetParent(slots[j + weaponSlotAmount + inventorySlotAmount].transform);
            itemObj.transform.position = slots[j + weaponSlotAmount + inventorySlotAmount].transform.position;
            itemObj.GetComponent<Image>().sprite = items[j + weaponSlotAmount + inventorySlotAmount].Sprite;
            itemObj.name = items[j + weaponSlotAmount + inventorySlotAmount].Title;

            }
        }

        ////////////////////////////////////////////////////////////////////////
    }

    public void SaveChest(GameObject chest)
    {
        for (int j = 0; j < chestSlotAmount; j++)
        {
            chest.GetComponent<ChestStorage>().itemsInChest[j] = items[j + weaponSlotAmount + inventorySlotAmount];
        }

        for (int j = 0; j < chestSlotAmount; j++)
        {
            Destroy(slots[(chestSlotAmount - j) + weaponSlotAmount + inventorySlotAmount - 1]);
            slots.Remove(slots[(chestSlotAmount - j) + weaponSlotAmount + inventorySlotAmount - 1]);
            items.Remove(items[(chestSlotAmount - j) + weaponSlotAmount + inventorySlotAmount - 1]);
        }

    }
}
