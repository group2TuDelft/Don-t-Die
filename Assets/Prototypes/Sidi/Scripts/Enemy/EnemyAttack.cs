using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
	 
    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
	EnemyMovement enemyMovement;
    //EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;
	float timeBetweenAttacks = 0.5f;
	int attackDamage = 10;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
		enemyMovement = GetComponent<EnemyMovement>();
        //enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(timer >= timeBetweenAttacks && playerInRange) //&& enemyHealth.currentHealth > 0)
        {
            Attack ();
        }

        if(playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
			enemyMovement.enabled = false;
        }
    }


    void Attack ()
    {
        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
