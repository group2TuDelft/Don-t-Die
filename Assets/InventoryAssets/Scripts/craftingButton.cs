using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class craftingButton : MonoBehaviour {

    // Access inventory:
    private Inventory inv;

    // Button Characteristics:
    public int id;
    // Costs of the items created
    private List<int> costs = new List<int>();
    private List<int> locations = new List<int>(new int[11]);
    ItemDatabase database;
    //
    [SerializeField] int AmountMade;
    [SerializeField] int woodCost;
    [SerializeField] int stoneCost;
    [SerializeField] int ironCost;
    [SerializeField] int coalCost;
    [SerializeField] int steelCost;
    [SerializeField] int gunpowderCost;
    [SerializeField] int carbonFiberCost;
    [SerializeField] int explosivesCost;
    [SerializeField] int alienFuelCost;
    [SerializeField] int alienCompositesCost;
    [SerializeField] int alienComputerChipCost;
    //
    private bool yayorneigh;
    
    void Start ()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        database = inv.GetComponent<ItemDatabase>();

        costs.Add(woodCost);
        costs.Add(stoneCost);
        costs.Add(ironCost);
        costs.Add(coalCost);
        costs.Add(steelCost);
        costs.Add(gunpowderCost);
        costs.Add(carbonFiberCost);
        costs.Add(explosivesCost);
        costs.Add(alienFuelCost);
        costs.Add(alienCompositesCost);
        costs.Add(alienComputerChipCost);
    }

    public void CraftingAttempt ()
    {
        yayorneigh = true;
        for (int i = 0; i < costs.Count; i ++)
        {

            locations[i] = CheckPrice(i + 1000, costs[i]);

            if (costs[i] != 0 && CheckPrice(i + 1000, costs[i]) == -1)
            {
                Debug.Log("Can not pay");
                yayorneigh = false;
            }
        }

        // If you arrive here you can afford the item.
        if (yayorneigh == true) { 
        Craft();
        }
    }

    // For Crafting Check if an item is affordable
    // input is an id of the resources and the cost and it will return a bool wether its affordable or not.
    public int CheckPrice(int id_item, int cost)
    {
        Item resource = database.FetchItemByID(id_item);

        for (int i = 0; i < inv.slots.Count; i++)
        {
            if (inv.items[i].ID == resource.ID)
            {
                if (inv.slots[i].transform.GetChild(0).GetComponent<itemData>().amount >= cost)
                {
                    return i;
                }
                else
                {
                return -1;
                }
            }
        }

        return -1;
    }

    public void Craft()
    {
        // Get Item Being Crafted
        // Delete used resources
        for (int i = 0; i < costs.Count; i++)
        {
            if (costs[i] != 0)
            {
            inv.DeleteItem(1000 + i,costs[i]);
            }
        }

        // Create the item
        for (int i = 0; i < AmountMade; i++) {
            inv.AddItem(id);
        }
    }
}
