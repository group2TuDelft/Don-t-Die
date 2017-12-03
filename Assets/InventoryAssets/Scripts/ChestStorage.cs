using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestStorage : MonoBehaviour
{
    public int id;
    public List<Item> itemsInChest = new List<Item>();

    void Start ()
    {
    for (int i = 0; i < 8; i++)
        { 
        itemsInChest.Add(new Item());
        }
    }

}
