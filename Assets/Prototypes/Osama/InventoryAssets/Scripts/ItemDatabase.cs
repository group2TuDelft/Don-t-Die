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
        Debug.Log(FetchItemByID(0).Description);
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
            dataBase.Add(new Item((int)itemData[i]["id"], (string)itemData[i]["title"], (int)itemData[i]["value"],
                (int)itemData[i]["stats"]["power"], (int)itemData[i]["stats"]["defence"], (int)itemData[i]["stats"]["vitality"],
                itemData[i]["description"].ToString(), (bool)itemData[i]["stackable"], (int)itemData[i]["rarety"],
                itemData[i]["slug"].ToString()));
        }
    }
}

// Make a Class for items within the game.
public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public int Power { get; set; }
    public int Defence { get; set; }
    public int Vitality { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }

    // this handle the actual storing the values of an item.
    public Item(int id, string title, int value, int power, int defence, int vitality, string description, bool stackable, int rarity, string slug )
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;
        this.Power = power;
        this.Defence = defence;
        this.Vitality = vitality;
        this.Description = description;
        this.Stackable = stackable;
        this.Rarity = rarity;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);
    }

    // This is so you will recognize an item which doesnt recieve the correct input, then the item will have an id of -1.
    public Item()
    {
        this.ID = -1;
    }
}