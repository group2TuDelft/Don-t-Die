using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
	 
    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
	EnemyMovement enemyMovement;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;
	float timeBetweenAttacks = 0.5f;
	int attackDamage = 5;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
		enemyMovement = GetComponent<EnemyMovement>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = player.GetComponent <Animator> ();
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

		if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack ();
        }

		if(playerHealth.getCurrentHealth() <= 0)
        {
			anim.SetBool ("PlayerDead", true);
			enemyMovement.enabled = false;
        }
    }


    void Attack ()
    {
        timer = 0f;

		if(playerHealth.getCurrentHealth() > 0)
        {
			playerHealth.TakeDamage (attackDamage, this.name);
        }
    }
}
