using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAnimator : MonoBehaviour {

	public float shootdistance = 50f;
	public bool InRange = false;
	public float Distance;

	private Animator anim;

	GameObject Player;
	PlayerHealth playerHealth;
	EnemyHealth enemyeHealth;

	void Awake ()
	{
		Player = GameObject.FindWithTag ("Player");
		playerHealth = Player.GetComponent<PlayerHealth> ();
		enemyeHealth = GetComponent<EnemyHealth> ();
	}
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		Distance = Vector3.Distance (Player.transform.position, gameObject.transform.position);
		InRange = Distance < shootdistance;
		SetAnimation ();
	}

	void SetAnimation () {
		
		if (enemyeHealth.currentHealth > 0) {
			if (playerHealth.getCurrentHealth () > 0) {
				anim.SetBool ("AnimWalk", true);
				if (InRange == true) {
					anim.SetBool ("AnimShooting", true);
				} else {
					anim.SetBool ("AnimShooting", false);
				}
			} else {
				anim.SetBool ("AnimWalk", false);
			}
		} else {
			anim.SetBool ("AnimDead", true);
		}
	}
}
