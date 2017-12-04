using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;



public class PlayerHealth : MonoBehaviour
{



	Image fullHeart;
	Image heart75;
	Image heart50;
	Image heart25;
	Animator anim;
	//Slider healthSlider;
	PlayerMovement playerMovement;
	PlayerShooting playerShooting;
	bool isDead;
	bool damaged;
	int startingHealth = 100;
	int currentHealth;
	float flashSpeed = 5f;

    void Awake ()
	{
		fullHeart = GameObject.Find("HeartFull").GetComponent<UnityEngine.UI.Image>();
		heart75 = GameObject.Find("Heart75").GetComponent<UnityEngine.UI.Image>();
		heart50 = GameObject.Find("Heart50").GetComponent<UnityEngine.UI.Image>();
		heart25 = GameObject.Find("Heart25").GetComponent<UnityEngine.UI.Image>();
		//healthSlider = GameObject.Find ("HealthSlider").GetComponent<UnityEngine.UI.Slider>();
		playerMovement = GetComponent<PlayerMovement> ();
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
			fullHeart.enabled = false;
			heart75.enabled = true;
		} else if (currentHealth < 50 && currentHealth >= 25) {
			heart75.enabled = false;
			heart50.enabled = true;
		} else if (currentHealth < 25) {
			heart50.enabled = false;
			heart25.enabled = true;
		} else if (isDead) {
			heart25.enabled = false;
		}
		//healthSlider.value = currentHealth;

		if (currentHealth <= 0 && !isDead) {
			Die ();
		}
    }

	public int getCurrentHealth()
	{
		return currentHealth;
	}

    void Die ()
    {
		isDead = true;

		anim.SetTrigger ("Die");


		playerShooting.DisableEffects ();
		playerMovement.enabled = false;
		playerShooting.enabled = false;
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
}
