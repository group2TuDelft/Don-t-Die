﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;



public class PlayerHealth : MonoBehaviour
{
	float timer = 0.0f;


	Animator anim;
	Slider healthSlider;
	PlayerMovement playerMovement;
	PlayerShooting playerShooting;
	PlayerAnimator playerAnimator;
	bool isDead;
	bool damaged;
	int startingHealth = 200;
	int currentHealth;
	float flashSpeed = 5f;
	[SerializeField] Sprite [] hearts;
	[SerializeField] Image heartImage;

    void Awake ()
	{
		healthSlider = GameObject.Find ("HealthSlider").GetComponent<UnityEngine.UI.Slider>();
		playerMovement = GetComponent<PlayerMovement> ();
		playerAnimator = GetComponent<PlayerAnimator> ();
		playerShooting = GetComponentInChildren<PlayerShooting> ();
		anim = GetComponent<Animator> ();
		currentHealth = startingHealth;

    }


    void Update ()
    {
		
    }


    public void TakeDamage (int amount)
    {
		damaged = true;
		currentHealth -= amount;
		if (currentHealth < 75 && currentHealth >= 50) {
			heartImage.sprite = hearts [1];
		} else if (currentHealth < 50 && currentHealth >= 25) {
			heartImage.sprite = hearts [2];
		} else if (currentHealth < 25) {
			heartImage.sprite = hearts [3];
		}
		
		healthSlider.value = currentHealth;

		if (currentHealth <= 0 && !isDead) {
			Die ();
            FindObjectOfType<GameManager>().GameOver();
			heartImage.enabled = false;
		}
    }

	public int getCurrentHealth()
	{
		return currentHealth;
	}

    void Die ()
    {
		playerShooting.DisableEffects ();
		playerMovement.enabled = false;
		playerShooting.enabled = false;

		timer += Time.deltaTime;

		if (timer >= Time.deltaTime) {
			anim.SetBool ("PlayerDead", false);
			playerAnimator.enabled = false;
			isDead = true;
		} else {
			anim.SetBool ("PlayerDead", true);
		}

			
    }
		
}
