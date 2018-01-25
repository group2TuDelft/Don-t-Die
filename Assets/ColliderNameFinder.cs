using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderNameFinder : MonoBehaviour
{

    GameObject collidername;
    public string componentname;



    public void scriptname(GameObject obj)
    {

        if (componentname == "1")
        {
            Humanoid_AI_Easy enemyHealth = obj.GetComponent<Humanoid_AI_Easy>();
            enemyHealth.TakeDamage(30);
        }
        if (componentname == "2")
        {
            Hivemind_AI_Easy enemyHealth = obj.GetComponent<Hivemind_AI_Easy>();
            enemyHealth.TakeDamage(30);
        }
        if (componentname == "3")
        {
            Minion_AI enemyHealth = obj.GetComponent<Minion_AI>();
            enemyHealth.TakeDamage(30);
        }
        if (componentname == "4")
        {
            Zergling_AI_Hard enemyHealth = obj.GetComponent<Zergling_AI_Hard>();
            enemyHealth.TakeDamage(30);
        }
    }
}
