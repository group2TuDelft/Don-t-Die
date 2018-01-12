using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script will be used to couple the inventory database to the UI and make it visable in game.
public class Inventory : MonoBehaviour
{
    // Required UI objects - Inventory
    GameObject inventoryPanel;
    GameObject WeaponSlotPanel;
    GameObject InventorySlotPanel;

    // Required UI objects - Chest/Storage:
    private GameObject ChestPanel;
    private GameObject Canvas;

    ItemDatabase database;

    // Universal inventory item and slots
    private GameObject inventorySlot;
    private GameObject inventoryItem;

    // Slot Amounts
    private int weaponSlotAmount;
    private int chestSlotAmount;
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
        Canvas = GameObject.Find("MainCanvas");
        ChestPanel = Canvas.transform.GetChild(1).GetChild(0).gameObject;
        inventoryItem = Resources.Load<GameObject>("Inventory/Prefabs/Item");
        inventorySlot = Resources.Load<GameObject>("Inventory/Prefabs/ItemSlot");
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

        // Adding Initial Items.
        AddItem(0);
        AddItem(4);
        AddItem(2);
        AddItem(5);
        AddItem(7);
        AddItem(1000);
        AddItem(1000);
        AddItem(1000);
        AddItem(1001);
        AddItem(1001);  
        AddItem(1002);
        AddItem(1003);
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

    public void DeleteItem (int id, int amount)
    {
        Item itemToDel = database.FetchItemByID(id);

        if (CheckItemInInventory(itemToDel))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == id)
                {
                    itemData data = slots[i].transform.GetChild(0).GetComponent<itemData>();
                    data.amount = data.amount - amount;

                    if (data.amount == 0)
                    {

                        items.Remove(items[i]);
                        items.Insert(i, new Item());
                        
                        DestroyImmediate(slots[i].transform.GetChild(0).gameObject);
                    }
                    else
                    { 
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    }
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

    // Function that checks if an item is in the inventory, by id.
    bool CheckItemInInventoryByID(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == id)
            {
                return true;
            }
        }
        return false;
    }

    // Chest Initialization: ///////////////////////////////////////////////////////
    // This function creates any storage panel and destroys it upon opening     ////
    // and closing. The chests themself will contain data about their inventory ////
    // This will be loaded in upon opening them.                                ////
    ////////////////////////////////////////////////////////////////////////////////

    public void InitializeChest(GameObject chest)
    {

        chestSlotAmount = 8;
        
        for (int j = 0; j < chestSlotAmount; j++)
        {   
            // Initialize Slots
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[j + weaponSlotAmount + inventorySlotAmount].GetComponent<inventorySlot>().id = j + weaponSlotAmount + inventorySlotAmount;
            slots[j + weaponSlotAmount + inventorySlotAmount].transform.SetParent(ChestPanel.transform);
            
            // Load items from list in chestStorage
            items[j + weaponSlotAmount + inventorySlotAmount] = chest.GetComponent<ChestStorage>().itemsInChest[j];

            // Set Correct Amount
            if (items[j + weaponSlotAmount + inventorySlotAmount].ID != -1) { 

            // Draw Items if there are any
            GameObject itemObj = Instantiate(inventoryItem);
            itemObj.GetComponent<itemData>().item = items[j + weaponSlotAmount + inventorySlotAmount];
            itemObj.GetComponent<itemData>().slotid = j + weaponSlotAmount + inventorySlotAmount;
            itemObj.transform.SetParent(slots[j + weaponSlotAmount + inventorySlotAmount].transform);
            itemObj.transform.position = slots[j + weaponSlotAmount + inventorySlotAmount].transform.position;
            itemObj.GetComponent<Image>().sprite = items[j + weaponSlotAmount + inventorySlotAmount].Sprite;
            itemObj.name = items[j + weaponSlotAmount + inventorySlotAmount].Title;
            
            // Loads and displays the amount of the items
            itemData data = slots[j + weaponSlotAmount + inventorySlotAmount].transform.GetChild(0).GetComponent<itemData>();
            data.amount = chest.GetComponent<ChestStorage>().amountInChest[j];
                if (data.amount > 1) { 
                data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                }
            }
        }
    }

    // Stores All the changes made to the chest slots into the chest data and destroys the chestUI.
    public void SaveChest(GameObject chest)
    {
        for (int j = 0; j < chestSlotAmount; j++)
        {
            chest.GetComponent<ChestStorage>().itemsInChest[j] = items[j + weaponSlotAmount + inventorySlotAmount];

            if (items[j + weaponSlotAmount + inventorySlotAmount].ID != -1)
            {
                itemData data = slots[j + weaponSlotAmount + inventorySlotAmount].transform.GetChild(0).GetComponent<itemData>();
                chest.GetComponent<ChestStorage>().amountInChest[j] = data.amount;
            }
        }

        for (int j = 0; j < chestSlotAmount; j++)
        {
            Destroy(slots[(chestSlotAmount - j) + weaponSlotAmount + inventorySlotAmount - 1]);
            slots.Remove(slots[(chestSlotAmount - j) + weaponSlotAmount + inventorySlotAmount - 1]);
            items.Remove(items[(chestSlotAmount - j) + weaponSlotAmount + inventorySlotAmount - 1]);
        }

    }
}
