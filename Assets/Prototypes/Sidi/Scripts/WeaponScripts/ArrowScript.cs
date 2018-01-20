using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    public int damage = 50;

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Enemy")
        {
            Humanoid_AI_Easy enemyHealth = col.GetComponent<Humanoid_AI_Easy>();
            enemyHealth.TakeDamage(damage);
        }
    }

}
