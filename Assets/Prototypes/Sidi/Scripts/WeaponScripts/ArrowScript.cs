using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    public int damage = 50;

    void OnTriggerEnter(Collider col)
    {

        if (col.tag == "Enemy")
        {
            col.GetComponent<ColliderNameFinder>().scriptname(col.gameObject);
        }
    }

}
