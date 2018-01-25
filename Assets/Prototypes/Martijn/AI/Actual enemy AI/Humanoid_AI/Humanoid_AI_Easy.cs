using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Humanoid_AI_Easy : MonoBehaviour {
    [SerializeField]
    private NavMeshAgent navmesh;
    private Transform this_tr;
    private Transform player_tr;
    private Animator animator;
    private AudioManager audiomanager;
    CapsuleCollider capsuleCollider;
    RuntimeAnimatorController ac;
    private GameObject playerscript;

    public int scoreValue = 10;
    public int starting_health = 300; //health waarmee de enemy begint
    public int current_health = 1;
    public float agro_distance = 30f; // afstand waarbij die je ziet
    public float shoot_range = 60f; // Maximale schiet afstand
    public float shootingrotationspeed = 2f; // Snelheid waarbij de enemy draait als die aan het schieten is
    public float de_agro_range = 100f; // afstand waarbij die stopt met volgen
    public float find_cover_range = 10f; // Als een object binnen deze afstand ligt, dan gaat die daar achter dekking zoeken
    public float reloadtime = 1f; // Seconden tussen elk schot

    public float normalspeed = 3f; // Loop snelheid als die geen agro heeft
    public float agrospeed = 5f; // Snelheid als die de player achtervolgd
    public float shootingspeed = 2f; // Snelheid als die aan het schieten is

    public float audiocooldownmin = 2f;
    public float audiocooldownmax = 10f;
    public float audiocooldown = 0f; //Tijd waar tussen optionele sound effects mogen worden afgespeeld

    public float shootsinctimer = 0f;
    public float firstshotsinc = 0.2f; // Al deze dingen werken samen om animatie en schieten goed samen te laten lopen
    public float secondshotsinc = 0.4f;
    public bool firstshotshot = false;
 

    private bool seeing = false; // Boolean of deze enemy de player ziet
    private bool agro = false; // Boolean of deze enemy de player probeert aan te vallen
    private bool shooting = false; // Is die nu aan het schieten of niet
    private bool foundcover = false; // Boolean of deze enemy een valid cover heeft gevonden
    private bool isdead = false; // Is this enemy dead?

    public bool medium = false;
    public bool hard = false;

    private bool findcoverloop = true; // Boolean die de radius find voor find cover functie bepaald
    private float findcoverradius = 0f; // Float die in de findcoverloop steeds groter wordt

    private float timer = 0f; //General timer voor alles in deze class
    private float walkcooldowntimer = 1f;
    public float walkcooldown = 3f; // Tijd waartussen de enemy een nieuw random walk punt kiest
    private float shoottimer = 0f; // Timer die gebruikt wordt om shoot te triggeren
    private float audiotimer = 0f; // Timer om audio te triggeren

    public float despawntime = 1.5f;

    // Bullet Stuff

    public int damagePerShot = 20;
    public int BulletSpeed = 20;
    public float range = 100f;
    public GameObject bulletPrefab;

    //[SerializeField]
    public Transform bulletSpawn;
    public GameObject Humanoid;
    int shootableMask;
    float effectsDisplayTime = 0.2f;
    bool bulletHit = false;



    // Use this for initialization
    void Start () {

        shootableMask = LayerMask.GetMask("Shootable");
        current_health = starting_health;
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = this.GetComponent<Animator>();
        navmesh = this.GetComponent<NavMeshAgent>();
        this_tr = this.GetComponent<Transform>();
        player_tr = GameObject.Find("Player").GetComponent<Transform>();
        playerscript = GameObject.Find("Player");

        audiomanager = FindObjectOfType<AudioManager>();
        ac = animator.runtimeAnimatorController;

        navmesh.speed = normalspeed;
    }

    void CheckInLineOfSight() // Deze checkt of de enemy de player ziet
    {
        RaycastHit hitInfo;
        Vector3 direction = player_tr.position - this_tr.position;
        if (Physics.Raycast(this_tr.position, direction, out hitInfo, agro_distance)) // nog angle erin zetten
        {
            if (hitInfo.collider.tag == "Player") // Als die iets hit, en de hit is de player, doe dit
            {
                //Debug.Log("Ik zie je nu");
                seeing = true;
            }
            // kan hier nog muren kapot maken in zetten
            else // Als die iets hit, maar het is niet player, doe dit
            {
                //Debug.Log("Ik zie je niet nu");
                seeing = false;
            }
        }
    }

    void CheckAgro() // Deze functie checkt of de enemy de player nog actief gaat aanvallen
    {
        if (seeing && !agro)
        {
            agro = true;
            audiomanager.RandomPlay("HumanoidSeePlayer");
            navmesh.speed = agrospeed;
        }
        else if (agro && (player_tr.position - this_tr.position).magnitude > de_agro_range)
        {
            agro = false;
            navmesh.speed = normalspeed;
        }
    }

    void DecideDestination() //Kiest wat voor een loop functie de enemy gaat nemen
    {
        if (agro && !foundcover && medium) // Moet nog agro && foundcover bij
        {
            FindCover();
        }
        else
        {
            RandomWalk();
        }
    }

    void FindCover() //Check for wall between you and player, is it there? hide behind it, of not, random walk - NOT DONE YET
    {
        NavMeshHit hit;
        if (navmesh.FindClosestEdge(out hit))
        {
            Collider[] hitColliders = Physics.OverlapSphere(hit.position, 10);
            //Debug.Log(hitColliders[0]);
            //Debug.Log(hitColliders[1]);
            if (hit.distance < find_cover_range && foundcover == false)
            {
                animator.SetBool("AnimWalk", true);
                foundcover = true;
                navmesh.destination = hit.position;
                
            }
            else
            {
                RandomWalk();
            }
        }
    }

    void FindCover2()
    {
        Collider[] hitColliders = new Collider[0];
        while (findcoverloop) // Loop om de meest dichtbij cover te vinden tot een max afstand
        {
            findcoverradius++;
            hitColliders = Physics.OverlapSphere(this_tr.position, findcoverradius);
            if (hitColliders.Length > 0)
            {
                findcoverloop = false;
                foundcover = true;
            }
            else
            {
                if(findcoverradius > find_cover_range)
                {
                    findcoverloop = false;
                    foundcover = false;
                }
            }

        }

        Collider cover = hitColliders[0];
        //Debug.Log(cover);
    }

    void RandomWalk() // Picks random points to walk to
    {
        if ((this_tr.position - navmesh.destination).magnitude < 1f)
        { //  < dan tollerantie, anders waggelt die om de waypoint
            navmesh.destination = this_tr.position;
            animator.SetBool("AnimWalk", false);
            walkcooldowntimer += Time.deltaTime;
            if (walkcooldowntimer > walkcooldown)
            {
                walkcooldowntimer = 0f;
                animator.SetBool("AnimWalk", true);
                navmesh.destination += new Vector3(Random.Range(-20.0f, 20f), 0f, Random.Range(-20.0f, 20f));
 
            }

        }
    }

    void Shoot() // Tries to shoot at the player if possible, and adjusts speed accordingly
    {
        shoottimer += Time.deltaTime;
        if (agro && seeing && !isdead)
        {
            if ((player_tr.position - this_tr.position).magnitude < shoot_range)
            {
                navmesh.speed = shootingspeed;

                if (shoottimer > reloadtime)
                {
                    if (!firstshotshot)
                    {

                    }
                    animator.SetBool("AnimShooting", true);
                    shooting = true;
                    shootsinctimer += Time.deltaTime;
                    if(shootsinctimer >= firstshotsinc && !firstshotshot)
                    {
                        Fire(1);
                        audiomanager.RandomPlay("plasmarifle");
                        firstshotshot = true;
                        animator.SetBool("AnimShooting", false); 
                    }
                    if(shootsinctimer >= secondshotsinc)
                    {
                        Fire(2);
                        audiomanager.RandomPlay("plasmarifle");
                        shootsinctimer = 0f;
                        shoottimer = 0f;
                        firstshotshot = false;
                    }
                }
                else
                {
                    animator.SetBool("AnimShooting", false);
                }
            }
            else
            {
                shooting = false;
                animator.SetBool("AnimShooting", false);
                navmesh.speed = agrospeed;
            }
            
        }
        else
        {
            shooting = false;
            animator.SetBool("AnimShooting", false);
        }
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 lookPos = target - this_tr.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, shootingrotationspeed);
    }

    private void PlayAudio()
    {
        audiotimer += Time.deltaTime;
        if (audiotimer > audiocooldown)
        {
            audiotimer = 0f;
            audiocooldown = Random.Range(audiocooldownmin, audiocooldownmax);
            // Speel random geluid af
        }

    }
    // Update is called once per frame
    void Update () {
        CheckInLineOfSight();
        CheckAgro();
        DecideDestination();
        if (shooting)
        {
            FaceTarget(player_tr.position);
        }
        Shoot();
        PlayAudio();
		
	}

    public void TakeDamage(int amount)
    {
        if (isdead)
            return;

        current_health -= amount;
        if (current_health <= 0)
        {
            Death();
        }

    }

    void Death()
    {
        if (!isdead)
        {
            animator.SetBool("AnimDead", true);
            audiomanager.RandomPlay("HumanoidDeath");
            Destroy(this.gameObject, despawntime);
        }
        isdead = true;
        navmesh.destination = this_tr.position;
        capsuleCollider.isTrigger = true;

        ScoreManager.score += scoreValue;

    }
    void Fire(int shot)
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet\
        if (hard)
        {
            if (shot == 1)
            {
            bullet.GetComponent<Rigidbody>().velocity = (player_tr.position - bullet.transform.position).normalized * BulletSpeed;
            }
            else
            {
                float tijd = (bullet.transform.position - player_tr.position).magnitude / BulletSpeed;
                Vector3 estamtionplayer = player_tr.position + (player_tr.forward * playerscript.GetComponent<PlayerMovement>().speed * tijd);
                bullet.GetComponent<Rigidbody>().velocity = (estamtionplayer - bullet.transform.position).normalized * BulletSpeed;

            }


        }
        else //normaal schieten
        {
            bullet.GetComponent<Rigidbody>().velocity = (player_tr.position - bullet.transform.position).normalized * BulletSpeed;
        }


        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
}
