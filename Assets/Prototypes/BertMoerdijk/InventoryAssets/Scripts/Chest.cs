using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    private GameObject ChestPanel;
    private GameObject Canvas;
    private GameObject ActiveChest;
    private Inventory inv;
    private Crafting craftingWindows;

    // Use this for initialization
    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        Canvas = GameObject.Find("MainCanvas");
        ChestPanel = Canvas.transform.GetChild(1).GetChild(0).gameObject;
        craftingWindows = inv.GetComponent<Crafting>();
    }

    public void interactChest (GameObject interactedObject)
    {
        if (ChestPanel.activeSelf == true)
        {
            inv.SaveChest(ActiveChest);
            ChestPanel.gameObject.SetActive(false);
        }
        else
        {
            ActiveChest = interactedObject;
            ChestPanel.gameObject.SetActive(true);
            inv.InitializeChest(interactedObject);
        }
    }

	public void close (GameObject interactedObject)
	{
		inv.SaveChest(ActiveChest);
		ChestPanel.gameObject.SetActive(false);
	}
}