using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_dead_on_click : MonoBehaviour {

    public static bool playerDead = false;

    void OnMouseDown()
    {
        playerDead = true;
        FindObjectOfType<GameManager>().GameOver();
        
    }
}
