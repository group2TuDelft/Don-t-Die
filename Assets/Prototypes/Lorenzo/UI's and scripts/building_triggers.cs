using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class building_triggers : MonoBehaviour
{

    public GameObject roof;

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            roof.SetActive(false);
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            roof.SetActive(true);
        }
        
    }
}
