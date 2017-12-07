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
    public GameObject WorkBenchPanel;
    public GameObject FurnaceAndAnvil;
    public GameObject FactoryChainBelt;
    public GameObject ThreeDPrinter;
    public GameObject WeirdAlienMachine;

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
