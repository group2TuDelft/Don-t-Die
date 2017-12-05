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

	Animator anim;
	AudioSource enemyAudio;
	CapsuleCollider capsuleCollider;

	float sinkSpeed = 2.5f;
	bool isDead;
	bool isSinking;

    void Awake ()
    {
		anim = GetComponent<Animator> ();
		enemyAudio = GetComponent<AudioSource> ();
		capsuleCollider = GetComponent<CapsuleCollider> ();

		currentHealth = startingHealth;
    }


    void Update ()
    {
		if (isSinking) {
			transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
		}
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

		anim.SetTrigger ("Die");
	
		ScoreManager.score += scoreValue;
		StartSinking ();
    }


    public void StartSinking ()
    {
		GetComponent <NavMeshAgent> ().enabled = false;
		GetComponent <Rigidbody> ().isKinematic = true;
		isSinking = true;

		Destroy (gameObject, 2f);
    }

	public void RestartLevel ()
	{
		SceneManager.LoadScene (0);
	}

}
