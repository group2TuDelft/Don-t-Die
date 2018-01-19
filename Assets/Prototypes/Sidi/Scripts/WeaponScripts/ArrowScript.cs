using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    public int damage = 50;

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Enemy")
        {
            EnemyHealth enemyHealth = col.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage);
        }
    }

}
