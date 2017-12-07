using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid_AI_Easy_NonNavMesh : MonoBehaviour {

    private Transform thistr;
    private Transform playertr;
    private Rigidbody thisrb;

    private float shoottimer = 0f;
    public float reloadtime = 3f;
    public float agrodistance = 20f;

    private bool seeing = false;
    private bool agro = false;
    private bool behindcover = false;


	// Use this for initialization
	void Start () {
        thistr = this.GetComponent<Transform>();
        thisrb = this.GetComponent<Rigibbody>();
        playertr = Gameobject.find("Player").GetComponent<Transform>();

	}
	void CheckLineOfSight()
    {
        RayCastHit hit;
        Vector3 playerdirection = thistr.position - playertr.position;
        if (Physics.Raycast(thistr.position, playerdirection, out hit, agrodistance)) // Kan nog een hoek bij
        {
            if(hit.collider.tag == "Player")
            {
                seeing = true;
            }
            else
            {
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

    }

    void Shoot()
    {
        if (true) // Hier komt te staan, als player i
        {

            if (shoottimer > reloadtime)
            {
                //spawn bullet in direction of player
            }
        }

    }

	// Update is called once per frame
	void Update () {
        shoottimer += Time.deltaTime;
        CheckLineOfSight();
        CheckAgro();
        SetPath();
        Shoot();
		
	}
}
