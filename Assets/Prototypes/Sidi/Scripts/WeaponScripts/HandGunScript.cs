using UnityEngine;

public class HandGunScript : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.5f;
    public float range = 40f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    LineRenderer gunLine;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    bool bulletHit = false;

    // Ammo Related vars.
    public int ammo_id;
    private Inventory inv;
    private Animator anim;

    void Awake()
    {
        anim = GameObject.Find("Player").GetComponent<Animator>();
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            if (inv.CheckItemInInventoryByID(ammo_id))
            {
                anim.SetBool("AnimHasHandgun", true);
                inv.DeleteItem(ammo_id, 1);
                Shoot();
            }
            else
            {
                //anim.SetBool("AnimHasHandgun", false);
                Debug.Log("Out of Ammo");
            }
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }


    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot()
    {
        timer = 0f;

        gunLight.enabled = true;

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot); //shootHit.point);
            }
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
    
}
