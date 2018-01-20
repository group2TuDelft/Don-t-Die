using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearScript : MonoBehaviour {

    public int damage = 40;

    void OnTriggerEnter (Collider col)
    {

         if (col.gameObject.tag == "Enemy")
        {
            if (Input.GetButton("Fire1")) {
                 col.GetComponent<ColliderNameFinder>().scriptname(col.gameObject);
            }
        }
    }
}
