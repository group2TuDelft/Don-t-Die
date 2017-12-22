using UnityEngine;

public class HumanoidShooting : MonoBehaviour
{
	public int damagePerShot = 20;
	public int BulletSpeed = 20;
	public float range = 100f;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public GameObject Humanoid;

	float timer;

	int shootableMask;
	float effectsDisplayTime = 0.2f;
	bool bulletHit = false;

	private float time = 0.0f;
	public float timeBetweenBullets = 1.5f;


	void Awake ()
	{
		shootableMask = LayerMask.GetMask ("Shootable");
	}


	void Update ()
	{
		if (Humanoid.GetComponent<HumanoidAnimator> ().InRange == true) {
			time += Time.deltaTime;

			if (time >= timeBetweenBullets) {
				time = 0.0f;
				Fire ();
			}
		}
		
	}
	void Fire ()
	{
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * BulletSpeed ; 

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);        
	}
}
