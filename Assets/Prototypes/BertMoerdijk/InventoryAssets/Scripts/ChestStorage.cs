using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestStorage : MonoBehaviour
{
    public List<Item> itemsInChest = new List<Item>();
    public List<int> amountInChest = new List<int>();
    private float ratio_empty = 0.5f;
    private List<int> allowed_items = new List<int>() { 2, 4, 6, 8, 9 };

    ItemDatabase database;
    Inventory inv;

    void Start ()
    {

        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        database = inv.GetComponent<ItemDatabase>();
        
        for (int i = 0; i < 8; i++)
        {
            if (Random.value < ratio_empty) {
            Item itemToAdd = database.FetchItemByID(allowed_items[(int)Random.Range(0,allowed_items.Count)]);
            itemsInChest.Add(itemToAdd);
            amountInChest.Add(1);
            }
            else
            {
            itemsInChest.Add(new Item());
            amountInChest.Add(0);
            }

        }
    }

}
