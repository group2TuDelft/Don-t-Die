using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemDatabase : MonoBehaviour {

	private List<Item> dataBase = new List<Item>();

    void Start () 
	{
        dataBase.Add(new Item(1000, "Wood", "Wood, This can be used to craft items on a workbench", true, "resource", "wood"));
        dataBase.Add(new Item(1001, "Stone", "Stone, This can be used to craft items on a workbench", true, "resource", "stone"));
        dataBase.Add(new Item(1002, "Iron", "Iron", true,"resource", "iron"));
        dataBase.Add(new Item(1003, "Coal", "Coal", true,"resource", "coal"));
        dataBase.Add(new Item(1004, "Steel", "Steel", true,"resource", "steel"));
        dataBase.Add(new Item(1005, "Gunpowder", "boom",true, "resource", "gunpowder"));
        dataBase.Add(new Item(1006, "Carbon Fiber","Carbon Fiber, used for crafting in a 3d printer", true, "resource", "carbon_fiber"));
        dataBase.Add(new Item(1007, "Explosives", "Boom",  true,"resource", "explosives"));
        dataBase.Add(new Item(1008, "Alien Fuel", "Alien Fuel", true,  "resource", "alien_fuel"));
        dataBase.Add(new Item(1009, "Alien Composites", "Alien Composites", true,  "resource", "alien_composites"));
        dataBase.Add(new Item(1010, "Alien Computer Chip","Alien Computer Chip", true,  "resource", "alien_computer_chip"));
        dataBase.Add(new Item(0, "Rock", "A beautifull rock", false,  "weapon", "beginner_rock"));
        dataBase.Add(new Item(1,"Med Kit", "A med kit used for healing", false, "resource","med_kit"));
        dataBase.Add(new Item(24, "BlunderBuss Ammo", "Ammo for your Blunderbuss", true,  "ammo", "bluderbuss_ammo"));
        dataBase.Add(new Item(3, "Catapult", "Launches pebbles of 9.0 grams over 3 meters", false,  "weapon", "catapult"));
        dataBase.Add(new Item(4, "Spear", "Stab them with the pointy end", false,  "weapon", "spear"));
        dataBase.Add(new Item(18, "Pebbles", "Ammo for your catapult", true,  "ammo", "pebbles"));
        dataBase.Add(new Item(6, "Bow", "A beautifull bow",  false, "weapon", "bow"));
        dataBase.Add(new Item(26, "Arrows", "Ammo for your bow",  true,  "ammo", "arrows"));
        dataBase.Add(new Item(22, "Furnace Blower", "Needed to make use of a furnance and anvil", false,  "resource", "furnace_blower"));
        dataBase.Add(new Item(2, "Halbert", "A spear and an axe combined!", false,  "weapon", "halbert"));
        dataBase.Add(new Item(8, "Crossbow", "Xbow", false,  "weapon", "crossbow"));
        dataBase.Add(new Item(16, "Bolts", "Pointy sticks, used as ammo for your crossbow.", true,  "ammo", "bolts"));
        dataBase.Add(new Item(33, "Cogg", "A cogg needed to make use of the factory chain belt.", false,  "resource", "cogg"));
        dataBase.Add(new Item(15, "Axe", "Chop Chop", false,  "weapon", "axe"));
        dataBase.Add(new Item(11, "Revolver", "Shoots bullets", false, "weapon", "revolver"));
        dataBase.Add(new Item(17, "Revolver Ammo", "Ammo for your revolver.", true, "ammo", "revolver_ammo"));
        dataBase.Add(new Item(5, "SMG", "Maffia toys", false, "weapon", "smg"));
        dataBase.Add(new Item(19, "Shotgun Ammo","Ammo for your shotgun.", true, "ammo", "shotgun_ammo"));
        dataBase.Add(new Item(20, "Computer Chip", "Computer Chip, this is needed for using the 3D printer", false, "resource", "computer_chip"));
        dataBase.Add(new Item(12, "Shotgun", "Shotgun", false, "weapon", "shotgun"));
        dataBase.Add(new Item(10, "M1911", "Beautifull hand gun.", false,"weapon", "m1911"));
        dataBase.Add(new Item(23, "M1911 Ammo",  "Ammo for your m1911.", true, "ammo", "m1911_ammo"));
        dataBase.Add(new Item(9, "BlunderBuss", "Boom", false, "weapon", "blunderbuss"));
        dataBase.Add(new Item(25, "SMG ammo", "Ammo for your SMG.", true,"ammo", "smg_ammo"));
        dataBase.Add(new Item(7, "M4", "Just an M4.", false, "weapon", "m4"));
        dataBase.Add(new Item(27, "M4 Ammo", "Ammo for your M14.", true, "resource", "m4_ammo"));
        dataBase.Add(new Item(30, "13Aerj&*/**?/&@","Might have something to do with the weird alien machine", false,"resource", "weird_alien_machine_part"));
        dataBase.Add(new Item(13, "Alien HandGun", "It's a handgun but alien.",  false, "weapon", "scifihandgun"));
        dataBase.Add(new Item(14, "Big Alien Gun",  "Let's see what it does.",  false, "weapon", "scifibiggun"));
        dataBase.Add(new Item(34, "Alien Ammo",  "Stuff for alien guns.", true, "resource", "alien_ammo"));

    }

    public Item FetchItemByID(int id)
    {
        for (int i = 0; i < dataBase.Count; i++)
        {
            if (dataBase[i].ID == id)
            {
                return dataBase[i];
            }
        }

        Debug.Log("Requested Item does not excist with id:");
        Debug.Log(id);
        return null;
    }

}

// Make a Class for items within the game.
public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public string ItemType { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }

    // this handle the actual storing the values of an item.
    public Item(int id, string title, string description, bool stackable, string itemtype, string slug)
    {
        this.ID = id;
        this.Title = title;
        this.Description = description;
        this.Stackable = stackable;
        this.ItemType = itemtype;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/UI/Items/" + slug);
    }

    // This is so you will recognize an item which doesnt recieve the correct input, then the item will have an id of -1.
    public Item()
    {
        this.ID = -1;
    }
}