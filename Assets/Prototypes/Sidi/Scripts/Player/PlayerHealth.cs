using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;



public class PlayerHealth : MonoBehaviour
{



	//Animator anim;
	Slider healthSlider;
	PlayerMovement playerMovement;
	PlayerShooting playerShooting;
	bool isDead;
	bool damaged;
	int startingHealth = 100;
	int currentHealth;
	float flashSpeed = 5f;
	[SerializeField] Sprite [] hearts;
	[SerializeField] Image heartImage;

    void Awake ()
	{
		healthSlider = GameObject.Find ("HealthSlider").GetComponent<UnityEngine.UI.Slider>();
		playerMovement = GetComponent<PlayerMovement> ();
		playerShooting = GetComponentInChildren<PlayerShooting> ();
		//anim = GetComponent<Animator> ();
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
		heartImage.enabled = false;
		}
    }

	public int getCurrentHealth()
	{
		return currentHealth;
	}

    void Die ()
    {
		isDead = true;

		playerShooting.DisableEffects ();
		playerMovement.enabled = false;
		playerShooting.enabled = false;
    }
		
}
