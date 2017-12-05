using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// We use LitJson, which is a plugin that helps us storing data.
using LitJson;

public class ItemDatabase : MonoBehaviour {

	// Make a list of items
	private List<Item> dataBase = new List<Item>();
	JsonData itemData;

    //
	void Start () 
	{
        // Json mapper allows you to covnert data to json format and vice versa
		itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDatabase();
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

        Debug.Log("Requested Item does not excist");
        return null;
    }

    // Constructs the Database
    void ConstructItemDatabase ()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            dataBase.Add(new Item((int)itemData[i]["id"], (string)itemData[i]["title"],
                (int)itemData[i]["stats"]["damage"], (int)itemData[i]["stats"]["range"],
                itemData[i]["description"].ToString(), (bool)itemData[i]["stackable"], itemData[i]["itemtype"].ToString(),
                itemData[i]["slug"].ToString()));
        }
    }
}

// Make a Class for items within the game.
public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Damage { get; set; }
    public int Range { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public string ItemType { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }

    // this handle the actual storing the values of an item.
    public Item(int id, string title, int damage, int range, string description, bool stackable, string itemtype, string slug )
    {
        this.ID = id;
        this.Title = title;
        this.Damage = damage;
        this.Range = range;
        this.Description = description;
        this.Stackable = stackable;
        this.ItemType = itemtype;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);
    }

    // This is so you will recognize an item which doesnt recieve the correct input, then the item will have an id of -1.
    public Item()
    {
        this.ID = -1;
    }
}