using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will take care of the crafting system,
// It will have to:
// - Start up the crafting window with crafting buttons.
// - Handle button clicks to craft, which in turn will require:
//  > Search inventory function
//  > Handle insufficient materials
//  > Handle item deletion + creation (aka crafting)

public class Crafting : MonoBehaviour {

    // Required UI elements:
    private GameObject WorkBenchPanel;
    private GameObject FurnaceAndAnvil;
    private GameObject FactoryChainBelt;
    private GameObject ThreeDPrinter;
    private GameObject WeirdAlienMachine;
    private GameObject Canvas;

    void Start ()
    {
        Canvas = GameObject.Find("MainCanvas");
        WorkBenchPanel = Canvas.transform.GetChild(2).GetChild(0).gameObject;
        FurnaceAndAnvil = Canvas.transform.GetChild(2).GetChild(1).gameObject;
        FactoryChainBelt = Canvas.transform.GetChild(2).GetChild(2).gameObject;
        ThreeDPrinter = Canvas.transform.GetChild(2).GetChild(3).gameObject;
        WeirdAlienMachine = Canvas.transform.GetChild(2).GetChild(4).gameObject;
    }

    // Start up the crafting window with crafting buttons:
    public void ActivateWorkBench()
    {
        if (WorkBenchPanel.activeSelf)
        { 
        WorkBenchPanel.SetActive(false);
        }
        else
        {
            WorkBenchPanel.SetActive(true);
        }
    }

}
