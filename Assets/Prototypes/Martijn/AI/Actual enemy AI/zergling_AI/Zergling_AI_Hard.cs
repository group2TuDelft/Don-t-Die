using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zergling_AI_Hard : MonoBehaviour
{

    public NavMeshAgent navmesh;
    public Transform enemytr;
    public Transform playertr;
    public GameObject Howled_Zergling;

    public float agrodistance = 25f; // Afstand waarbij de enemy je "ziet"  10-20-30
    public float agroangle = 90f;    // De hoek waarbij de enemy je "ziet"  40 - 60 - 120
    public float smellingdistance = 10f; //Afstand waarbij die je ruikt
    public float smellingdeagro = 100f; //Afstand waarbij enemy de gear verliest
    public bool howled = false;         // Mag deze enemy howlen
    private float timer = 0f;           // timer tot dat enemy nieuwe position kiest in RandomWalk()
  

    private bool smelling = false;
    private bool seeing = false;
    private bool chasing = false;



    // Use this for initialization
    void Start()
    {
        //navmesh = this.GetComponent<NavMeshAgent>();
        navmesh.destination = enemytr.position;
        //enemytr = this.GetComponent<Transform>();
        //playertr = ???.GetComponent<Transform>();
    }
    void Howl()
    {
        var spawn1 = Instantiate(Howled_Zergling);
        spawn1.transform.position = enemytr.position - new Vector3(10, 0, -10);

        //spawn1.NavMeshAgent.destination = playertr.position;
        var spawn2 = Instantiate(Howled_Zergling);
        spawn2.transform.position = enemytr.position - new Vector3(-10, 0, -10);
        //spawn2.navmesh.destination = playertr.position;

        //Howl animation
    }
    void RandomWalk()
    {
        if ((enemytr.position - navmesh.destination).magnitude < 4f)
        { //  < dan tollerantie, anders waggelt die om de waypoint
            timer += Time.deltaTime;
            if (timer > 3f)
            {
                navmesh.destination += new Vector3(Random.Range(-20.0f, 20f), 0f, Random.Range(-20.0f, 20f));
                timer = 0f;
            }

        }
    }
    void CheckAgro()
    {

        if ((smelling || seeing) && !howled)
        {
            Howl();
            howled = true;
        }
        if(smelling|| seeing)
        {
            chasing = true;
        
        }
        if (!smelling && !seeing)
        {
            chasing = false;

        }
    }
    void CheckSmelling()
    {
        if (!smelling)
        {
            if ((playertr.position - enemytr.position).magnitude < smellingdistance) // Als player in smelling distance komt, smelling is waar
            {
                // smelling animation
                smelling = true;
            }
        }
        if (smelling) // Als die de player ruikt, maar player gaat uit range, dan raakt die de gear kwijt
        {
            if ((playertr.position - enemytr.position).magnitude > smellingdeagro)
            {
                smelling = false;
            }
        }
    }

    void CheckInLineOfSight()
    {
        RaycastHit hitInfo;
        Vector3 direction = playertr.position - enemytr.position;
        if (Physics.Raycast(enemytr.position, direction, out hitInfo, agrodistance)) // nog angle erin zetten
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
    // Update is called once per frame
    void Update()
    {
        CheckInLineOfSight();
        CheckSmelling();
        CheckAgro();
        if (chasing)
        {
            navmesh.destination = playertr.position;

        }
        else
        {
            RandomWalk();
        }

    }
}
