using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


public class EnemyHealth : MonoBehaviour
{
	
	 
	public int scoreValue = 10;
	public int startingHealth = 100;
	public int currentHealth;

	AudioSource enemyAudio;
	CapsuleCollider capsuleCollider;
	bool isDead;

    void Awake ()
    {
		//anim = GetComponent<Animator> ();
		enemyAudio = GetComponent<AudioSource> ();
		capsuleCollider = GetComponent<CapsuleCollider> ();

		currentHealth = startingHealth;
    }


    void Update ()
    {

    }

	public void TakeDamage (int amount)//, Vector3 hitPoint)
    {
		if (isDead)
			return;

		currentHealth -= amount;
		if (currentHealth <= 0) {
			Death ();
		}

    }
		
    void Death ()
    {
		isDead = true;

		capsuleCollider.isTrigger = true;
	
		ScoreManager.score += scoreValue;
		Destroy (gameObject, 1.5f);

    }



	public void RestartLevel ()
	{

	}

}
