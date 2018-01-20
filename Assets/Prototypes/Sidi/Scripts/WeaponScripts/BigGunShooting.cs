using UnityEngine;

public class BigGunShooting : MonoBehaviour
{

    public float timeBetweenBullets;
    public float range = 40f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    

    public float sprayFactor = 0.05f;
    public int amountofshot = 5;
    RuntimeAnimatorController ac;
    public float animationoffset = 0.4f;
    private bool shot = false;

    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
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
        gunLight = GetComponent<Light>();
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        
        ac = anim.runtimeAnimatorController;

        // Finds the length of the shooting animation
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == "Shotgun_Shooting")
            {
                timeBetweenBullets = ac.animationClips[i].length / 2;
            }
        }
    }


    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            shot = false;
        }

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets - animationoffset && Time.timeScale != 0 && shot == false)
        {
            Debug.Log("Hier BigGunShooting" + ammo_id);
            if (inv.CheckItemInInventoryByID(ammo_id))
            {
                anim.SetBool("AnimHasGun", true);
                inv.DeleteItem(ammo_id, 1);
                for (int i = 0; i < amountofshot; i++)
                    { 
                    Fire(Random.value);
                }
                shot = true;
            }
            else
            {
                anim.SetBool("AnimHasGun", false);
                Debug.Log("Out of Ammo");
            }
        }

        if (timer >= timeBetweenBullets)
        {
            timer = 0f;
            shot = false;
            DisableEffects();
        }
    }


    public void DisableEffects()
    {
        gunLight.enabled = false;
    }

    void Fire(float rand)
    {
        gunLight.enabled = true;

        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.transform.Rotate(Vector3.one * sprayFactor * rand * 90);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * (range * (1 + rand));

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
}
