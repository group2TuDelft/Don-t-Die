using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Animations;
using UnityEngine.AI;

public class Zergling_AI_Hard : MonoBehaviour
{
    private NavMeshAgent navmesh;
    private Transform thistr;
    private Transform playertr;
    public GameObject Howled_Zergling;
    private Animator animator;
    private AudioManager audiomanager;

    public int scoreValue = 10;
    public int starthealth = 200;
    public int currenthealth = 1;
    private float timer = 0f;

    public float attackdistance = 2f; //Afstand waarbij Zergling aan kan vallen
    public float attackcooldown = 3f; //Tijd die tussen elke attack moet zitten

    public float agrodistance = 25f; // Afstand waarbij de enemy je "ziet"  10-20-30
    public float agroangle = 90f;    // De hoek waarbij de enemy je "ziet"  40 - 60 - 120

    public float smellingdistance = 10f; //Afstand waarbij die je ruikt
    public float smellingdeagro = 100f; //Afstand waarbij enemy de gear verliest

    public float walkingspeed = 10f;
    public float runningspeed = 20f;
    public float attackingspeed = 30f;
    public float newrandompointcooldown = 3f; // Idle tijd waarna die een nieuw punt kiest in Randomwalk()

    private float walktimer = 0f;           // timer tot dat enemy nieuwe position kiest in RandomWalk()
    private float attacktimer = 0f;     // Timer die tijd tussen aanvallen bij houdt
    private float audiotimer = 0f;      // Een timer om bij te houden hoe veel tijd tussen optionele audiofragmenten zijn gespeeld

    public float audiocooldownmax = 6f; // Max van de random range waar tussen random geluidsfragmenten worden gespeeld
    public float audiocooldownmin = 4f; // Min van dit hier boven
    private float audiocooldown = 0f; // De daadwerklijke cooldown

    private float despawntime = 1.5f;

    private bool smelling = false;
    private bool seeing = false;
    private bool chasing = false;
    public bool howled = false;         // Heeft deze unit al gehowld?
    private bool isdead = false;

    private void Awake()
    {
        navmesh = this.GetComponent<NavMeshAgent>();
        thistr = this.GetComponent<Transform>();
        playertr = GameObject.Find("Player").GetComponent<Transform>();
        animator = this.GetComponent<Animator>();
        audiomanager = FindObjectOfType<AudioManager>();
    }
    void Start()

    {
        navmesh.speed = walkingspeed;
        navmesh.destination = thistr.position;
        currenthealth = starthealth;
    }
    void Howl()
    {
        var spawn1 = Instantiate(Howled_Zergling);
        spawn1.transform.position = thistr.position - new Vector3(10, 0, -10);

        //spawn1.NavMeshAgent.destination = playertr.position;
        var spawn2 = Instantiate(Howled_Zergling);
        spawn2.transform.position = thistr.position - new Vector3(-10, 0, -10);
        //spawn2.navmesh.destination = playertr.position;

        //Howl animation
    }
    void RandomWalk()
    {
        if ((thistr.position - navmesh.destination).magnitude < 1f) //1f is alleen een tollerantie zodat die niet rondjes om goal gaat lopen
        { 
            navmesh.destination = thistr.position;
            animator.SetBool("AnimWalking", false);
            walktimer += Time.deltaTime;
            if (walktimer > newrandompointcooldown)
            {
                //audiomanager.Play("Idle");
                animator.SetBool("AnimWalking", true);
                navmesh.destination += new Vector3(Random.Range(-20.0f, 20f), 0f, Random.Range(-20.0f, 20f));
                walktimer = 0f;
            }

        }
    }
    void CheckAgro()
    {

        if ((smelling || seeing) && !howled)
        {
            animator.SetBool("AnimHowl", true);
            audiomanager.Play("Howl");
            Howl();
            howled = true;
        }
        if(smelling|| seeing)
        {
            chasing = true;
            animator.SetBool("AnimWalking", true);
            animator.SetBool("AnimRunning", true);
        
        }
        if (!smelling && !seeing)
        {
            animator.SetBool("AnimHowl", false);
            animator.SetBool("AnimRunning", false);
            chasing = false;

        }
    }
    void CheckSmelling()
    {
        if (!smelling)
        {
            if ((playertr.position - thistr.position).magnitude < smellingdistance) // Als player in smelling distance komt, smelling is waar
            {
                // smelling animation
                smelling = true;
            }
        }
        if (smelling) // Als die de player ruikt, maar player gaat uit range, dan raakt die de gear kwijt
        {
            if ((playertr.position - thistr.position).magnitude > smellingdeagro)
            {
                smelling = false;
            }
        }
    }

    void CheckInLineOfSight()
    {
        RaycastHit hitInfo;
        Vector3 direction = playertr.position - thistr.position;
        if (Physics.Raycast(thistr.position, direction, out hitInfo, agrodistance)) // nog angle erin zetten
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

    void CheckAttack()
    {
        if (((thistr.position - playertr.position).magnitude < attackdistance) && (attacktimer > attackcooldown)){
            attacktimer = 0f;
            Attack();
        }
        else
        {
            attacktimer += Time.deltaTime;
            animator.SetBool("AnimAttack", false);
        }
    }

    void Attack()
    {
        audiomanager.Play("Attack");
        animator.SetBool("AnimAttack", true);


    }
    // Update is called once per frame

    void PlayAudio() // Kiest een optioneel audiofragment om te spelen
    {
        if (audiotimer > audiocooldown)
        {
            audiotimer = 0f;
            audiocooldown = Random.Range(audiocooldownmin, audiocooldownmax);
            // Je kan kiezen tussen Idle, Agro, Pain
            if (!chasing)
            {
                audiomanager.Play("Idle");
            }
            else
            {
                if (starthealth / currenthealth < 0.1)
                {
                    audiomanager.Play("PainAgro");
                }
                else
                {
                    audiomanager.Play("Agro");
                }
            }
        }
        else
        {
            audiotimer += Time.deltaTime;
        }
    }
    void Update()
    {
        timer += Time.deltaTime;
        CheckInLineOfSight();
        CheckSmelling();
        CheckAgro();
        if (chasing)
        {
            navmesh.destination = playertr.position;
            CheckAttack();
        }
        else
        {
            RandomWalk();
        }
        if (timer > 8)
        {
            Death();
        }

    }
    public void TakeDamage(int amount)
    {
        if (isdead)
            return;

        currenthealth -= amount;
        if (currenthealth <= 0)
        {
            Death();
        }

    }

    void Death()
    {
        if (!isdead)
        {
            animator.SetBool("AnimDead", true);
            audiomanager.RandomPlay("ZerglingDeath");
            Destroy(this.gameObject, despawntime);
        }
        isdead = true;
        navmesh.destination = thistr.position;


        //capsuleCollider.isTrigger = true;

        //ScoreManager.score += scoreValue;

    }
}

