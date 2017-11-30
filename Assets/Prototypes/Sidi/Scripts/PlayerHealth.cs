using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;



public class PlayerHealth : MonoBehaviour
{
  
	public int startingHealth = 100;
	public int currentHealth;
	public Image damageImage;
	public float flashSpeed = 5f;
	public Color flashColour = new Color (1f, 0f, 0f, 0.1f);


	Animator anim;
	Slider healthSlider;
	AudioSource playerAudio;
	PlayerMovement playerMovement;
	PlayerShooting playerShooting;
	bool isDead;
	bool damaged;




    void Awake ()
	{
		healthSlider = GameObject.Find ("HealthSlider").GetComponent<UnityEngine.UI.Slider>();
		playerAudio = GetComponent<AudioSource> ();
		playerMovement = GetComponent<PlayerMovement> ();
		playerShooting = GetComponentInChildren<PlayerShooting> ();
		anim = GetComponent<Animator> ();
		currentHealth = startingHealth;

    }


    void Update ()
    {
		if (damaged) {
			damageImage.color = flashColour;
		} else {
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		damaged = false;
    }


    public void TakeDamage (int amount)
    {
		damaged = true;
		currentHealth -= amount;
		healthSlider.value = currentHealth;

		if (currentHealth <= 0 && !isDead) {
			Die ();
		}
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
