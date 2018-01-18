﻿using System.Collections;
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

    public string diffeculty {get; set;} //Bepaald de moeilijkheid van de enemy
    public float gametimer { get; set; } //Game timer die bepaalde instellingen van de enemies bepalen


    private float timer;
    public void setTimer(float time)
    {
        timer = time;
    }
    public float getTimer()
    {
        return timer;
    }

    public float Timer
    {
        get
        {
            return timer;
        }
        set
        {
            timer = value;
        }
    }


    public float starting_health = 300f; //health waarmee de ennemy begint
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

    private bool seeing = false; // Boolean of deze enemy de player ziet
    private bool agro = false; // Boolean of deze enemy de player probeert aan te vallen
    private bool shooting = false; // Is die nu aan het schieten of niet
    private bool foundcover = false; // Boolean of deze enemy een valid cover heeft gevonden

    private bool findcoverloop = true; // Boolean die de radius find voor find cover functie bepaald
    private float findcoverradius = 0f; // Float die in de findcoverloop steeds groter wordt

    private float walktimer = 0f; // Timer die gebruikt wordt om random walk te triggeren
    private float shoottimer = 0f; // Timer die gebruikt wordt om shoot te triggeren
    private float audiotimer = 0f; // Timer om audio te triggeren



    // Use this for initialization
    void Start () {
        animator = this.GetComponent<Animator>();
        navmesh = this.GetComponent<NavMeshAgent>();
        this_tr = this.GetComponent<Transform>();
        player_tr = GameObject.Find("Player").GetComponent<Transform>();
        audiomanager = FindObjectOfType<AudioManager>();

        navmesh.speed = normalspeed;
        Timer = 0.4f;
        setTimer(0.3f);
    }

    void CheckInLineOfSight() // Deze checkt of de enemy de player ziet
    {
        RaycastHit hitInfo;
        Vector3 direction = player_tr.position - this_tr.position;
        if (Physics.Raycast(this_tr.position, direction, out hitInfo, agro_distance)) // nog angle erin zetten
        {
            if (hitInfo.collider.tag == "Player") // Als die iets hit, en de hit is de player, doe dit
            {
                Debug.Log("Ik zie je nu");
                seeing = true;
            }
            // kan hier nog muren kapot maken in zetten
            else // Als die iets hit, maar het is niet player, doe dit
            {
                Debug.Log("Ik zie je niet nu");
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
        if (agro && (player_tr.position - this_tr.position).magnitude > de_agro_range)
        {
            agro = false;
            navmesh.speed = normalspeed;
        }
    }

    void DecideDestination() //Kiest wat voor een loop functie de enemy gaat nemen
    {
        if (agro && !foundcover) // Moet nog agro && foundcover bij
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
            Debug.Log(hitColliders[0]);
            Debug.Log(hitColliders[1]);
            if (hit.distance < find_cover_range && foundcover == false)
            {
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
        Debug.Log(cover);
    }

    void RandomWalk() // Picks random points to walk to
    {
        if ((this_tr.position - navmesh.destination).magnitude < 1f)
        { //  < dan tollerantie, anders waggelt die om de waypoint
            navmesh.destination = this_tr.position;
            animator.SetBool("AnimWalk", false);      
            walktimer += Time.deltaTime;
            if (walktimer > 3f)
            {
                animator.SetBool("AnimWalk", true);
                navmesh.destination += new Vector3(Random.Range(-20.0f, 20f), 0f, Random.Range(-20.0f, 20f));
                walktimer = 0f;
            }

        }
    }

    void Shoot() // Tries to shoot at the player if possible, and adjusts speed accordingly
    {
        shoottimer += Time.deltaTime;
        if (agro && seeing)
        {
            if ((player_tr.position - this_tr.position).magnitude < shoot_range)
            {
                shooting = true;
                animator.SetBool("AnimShooting", true);
                navmesh.speed = shootingspeed;

                if (shoottimer > reloadtime)
                {
                    Debug.Log("BANG"); // Dit moet nog echte schiet functie worden die player damaged
                    audiomanager.RandomPlay("plasmarifle");
                    shoottimer = 0f;
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
}