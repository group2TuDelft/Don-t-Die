using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_AI : MonoBehaviour
{


    private int scoreValue = 10;
    public int starting_health;
    public int current_health;
    public int meleedamage = 10;
    public float speed = 20f;

    public Vector3 goal;
    private Vector3 destination;
    public Vector3 hivemindposition;
    public Vector3 spawnpoint;

    public float maxrangefromhivemind = 20f;
    public float agrodistance = 20f;
    public float despawntime = 1.5f;
    public float attackrange = 6f;
    public float rotationspeed = 1f;

    private bool seeing;
    public bool agro;
    public bool isdead = false;

    public float attacktimer = 0f;
    public float attackcooldown = 2f;

    private Transform playertr;
    private Rigidbody thisrb;
    private Transform thistr;
    private Vector3 randomwalkmedian;
    private Vector3 randomwalkpoint;
    private float randomwalktimer = 1f;
    private float randomwalkchange = 0f;

    //private GameObject ParentHivemind;
    private AudioManager audiomanager;
    private Animator animator;
    private GameObject player;


    // Use this for initialization
    void Start()
    {
        randomwalkchange = Random.Range(2f, 3f);
        playertr = GameObject.Find("Player").GetComponent<Transform>();
        thisrb = this.GetComponent<Rigidbody>();
        thistr = this.GetComponent<Transform>();
        thisrb.constraints = RigidbodyConstraints.FreezeRotationX;
        audiomanager = FindObjectOfType<AudioManager>();
        animator = this.GetComponent<Animator>();
        player = GameObject.Find("Player");


    }

    void CheckLineOfSight()
    {
        RaycastHit hitInfo;
        Vector3 direction = playertr.position - this.transform.position;
        if (Physics.Raycast(this.transform.position, direction, out hitInfo, agrodistance)) // nog angle erin zetten
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
        else
        {
            seeing = false;
        }
    }

    void CheckAgro()
    {
        if (seeing && !agro)
        {
            //ParentHivemind.GetComponent<Hivemind_AI_Easy>().agro = true;
            agro = true;
        }


    }
    void SetDestinationSelf()
    {
        if (agro)
        {
            ChasePlayer();
        }
        else
        {
            RandomWalk();
        }
    }

    void ChasePlayer()
    {
        goal = playertr.position;
    }

    void RandomWalk()
    {
        if (((goal - thistr.position).magnitude < 1f) || ((hivemindposition - thistr.position).magnitude > maxrangefromhivemind))
        {
            //randomwalktimer += Time.deltaTime; Niet nodig
            if (true) // randomwalktimer > 3f
            {
                randomwalkmedian = hivemindposition + spawnpoint;
                randomwalktimer = 0f;
                goal = randomwalkmedian + new Vector3(Random.Range(-6.0f, 6.0f), 0f, Random.Range(-6.0f, 6.0f));
            }
        }
    }

    void SetPath()
    {
        RaycastHit hitInfo;
        Vector3 direction = playertr.position - thistr.position;
        if (Physics.Raycast(thistr.position, direction, out hitInfo))
        {
            if (hitInfo.collider.tag == "Player")
            {
                destination = playertr.position; //Destination is eindpunt, goal is de waypoints op de route die ze willen hebben
            }
            else
            {
                FindPath();
            }
        }
    }

    void FindPath()
    {

    }

    void MoveSelf()
    {
        // Straks van goal destination maken
        thisrb.AddForce((goal - thistr.position).normalized * speed * Time.deltaTime, ForceMode.VelocityChange);
        //thistr.forward = new Vector3 ((goal - thistr.position).x, 0, (goal - thistr.position).z);
        //thistr.RotateTowards(thistr.forward, (goal - thistr.position), Time.deltaTime*100);
        //kaas = Quaternion.LookRotation(thistr.forward, thistr.position - goal);
        //Quaternion.RotateTowards(thistr.rotation, kaas, Time.deltaTime*1000);
        //thistr.forward = ((goal - thistr.position) * 2);
        

    }

    void CheckAttack()
    {
        attacktimer += Time.deltaTime;
        if ((thistr.position - playertr.position).magnitude < attackrange && attacktimer > attackcooldown)
        {
            attacktimer = 0f;
            //playertr.GetComponent<PlayerHealth>().TakeDamage(meleedamage, "Minion");
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(meleedamage);

            int attacktype = Random.Range(1, 3);
            if (attacktype == 1)
            {
                audiomanager.RandomPlay("MinionsMelee");
                animator.SetBool("AnimAttack1", true);
                animator.SetBool("AnimAttack2", false);

            }
            else
            {
                audiomanager.RandomPlay("MinionsMelee");
                animator.SetBool("AnimAttack1", false);
                animator.SetBool("AnimAttack2", true);
            }
        }
        else
        {
            animator.SetBool("AnimAttack1", false);
            animator.SetBool("AnimAttack2", false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //CheckLineOfSight();
        //CheckAgro();
        SetDestinationSelf();
        FaceTarget(goal);
        //SetPath();
        CheckAttack();
        PlayAudio();
    }

    private void FixedUpdate()
    {
        MoveSelf();
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 lookPos = target - thistr.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationspeed);
    }

    void PlayAudio()
    {
        // Nog implementeren
    }

    public void TakeDamage(int amount)
    {
        if (isdead)
            return;
        audiomanager.RandomPlay("HivemindPain");
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
            audiomanager.RandomPlay("MinionsDeath");
            Destroy(this.gameObject, despawntime);
        }
        isdead = true;
        goal = thistr.position;
    }
}
