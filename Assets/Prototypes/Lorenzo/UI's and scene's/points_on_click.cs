using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class points_on_click : MonoBehaviour {

    public int points = 0;

    void OnMouseDown()
    {
        points = points + 10;
        Debug.Log("points: " + points);
    }
}
