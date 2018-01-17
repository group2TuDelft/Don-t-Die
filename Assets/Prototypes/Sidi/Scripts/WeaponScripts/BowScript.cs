using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowScript : MonoBehaviour {
    
    private float timeBetweenShot;
    public float animationoffset;
    public float range = 40f;
    public string animationName;
    public GameObject ArrowPrefab;
    public Transform ArrowSpawn;
    RuntimeAnimatorController ac;

    private bool shot = false;

    float timer;

    // Ammo Related vars.
    public int ammo_id;
    private Inventory inv;
    private Animator anim;

    void Awake()
    {
        anim = GameObject.Find("Player").GetComponent<Animator>();
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        ac = anim.runtimeAnimatorController;

        // Finds the length of the shooting animation
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == animationName) 
            {
                timeBetweenShot = ac.animationClips[i].length;
            }
        }
        
    }

    void Update()
    {

        if (Input.GetButton("Fire1"))
        {
            timer += Time.deltaTime;
        }
        else { 
        timer = 0f;
        shot = false;
        }

        if (Input.GetButton("Fire1") && timer >= timeBetweenShot - animationoffset && Time.timeScale != 0 && shot == false)
        {

            if (inv.CheckItemInInventoryByID(ammo_id))
            {
                anim.SetBool("AnimHasBow", true);
                inv.DeleteItem(ammo_id, 1);
                Fire();
            }
            else
            {
                anim.SetBool("AnimHasBow", false);
                Debug.Log("Out of Ammo");
            }
        }

        if (timer >= timeBetweenShot)
        {
            timer = 0f;
            shot = false;
        }


    }

    void Fire()
    {
        shot = true;

        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            ArrowPrefab,
            ArrowSpawn.position,
            ArrowSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * range;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

}
