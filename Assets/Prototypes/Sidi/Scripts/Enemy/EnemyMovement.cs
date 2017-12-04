using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
  
	GameObject Player;
	PlayerHealth playerHealth;
	EnemyHealth enemyeHealth;
	NavMeshAgent nav;

    void Awake ()
    {
		Player = GameObject.FindWithTag ("Player");
		playerHealth = Player.GetComponent<PlayerHealth> ();
		enemyeHealth = GetComponent<EnemyHealth> ();
		nav = GetComponent<NavMeshAgent> ();
    }


    void Update ()
    {	
		if(enemyeHealth.currentHealth > 0 && playerHealth.getCurrentHealth() > 0)
		{
			nav.SetDestination (Player.transform.position);
		}
		else
		{
			nav.enabled = false;
		}
    }
}
