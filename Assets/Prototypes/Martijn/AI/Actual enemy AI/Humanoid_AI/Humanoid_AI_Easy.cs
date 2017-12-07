using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid_AI_Easy : MonoBehaviour {
    private NavMeshAgent navmesh;
    private Transform this_tr;
    private Transform player_tr;

    public float agro_distance = 30f;
    public float de_agro_range = 100f;
    public float find_cover_range = 30f;

    private bool seeing = false;
    private bool agro = false;
    private float timer = 0f;

    // Use this for initialization
    void Start () {
        navmesh = this.GetComponent<NavMeshAgent>();
        this_tr = this.GetComponent<Transform>();
        player_tr = GameObject.Find("Player").GetComponent<Transform>();
		
	}

    void CheckInLineOfSight()
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

    void CheckAgro()
    {
        if (seeing && !agro)
        {
            agro = true;
        }
        if (agro && (player_tr.position - this_tr.position).magnitude > de_agro_range)
        {
            agro = false;
        }
    }

    void DecideDestination()
    {
        if (agro)
        {
            FindCover();
        }
        else
        {
            RandomWalk();
        }
    }

    void FindCover()
    {
        //Check for walkk between you and player, is it there? hide behind it, of not, random walk
        NavMeshHit hit;
        if (navmesh.FindClosestEdge(out hit))
        {
            if(hit.distance < find_cover_range)
            {
                navmesh.destination = hit.position;
            }
            else
            {
                RandomWalk();
            }
        }
    }

    void RandomWalk()
    {
        if ((this_tr.position - navmesh.destination).magnitude < 4f)
        { //  < dan tollerantie, anders waggelt die om de waypoint
            timer += Time.deltaTime;
            if (timer > 3f)
            {
                navmesh.destination += new Vector3(Random.Range(-20.0f, 20f), 0f, Random.Range(-20.0f, 20f));
                timer = 0f;
            }

        }
    }

    void Shoot()
    {
        Debug.Log("BANG");
    }
    // Update is called once per frame
    void Update () {
        CheckInLineOfSight();
        CheckAgro();
        DecideDestination();
        Shoot();
		
	}
}
