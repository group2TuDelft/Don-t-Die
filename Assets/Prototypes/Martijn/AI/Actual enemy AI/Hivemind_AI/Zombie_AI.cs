using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_AI : MonoBehaviour {

    public Vector3 goal;
    public bool agro { get; set; }
    public float speed = 20f;
    public Vector3 hivemindposition { get; set; }
    public Vector3 spawnpoint {get; set;}

    private Transform playertr;
    private Rigidbody thisrb;
    private Transform thistr;
    private Vector3 randomwalkmedian;
    private Vector3 randomwalkpoint;
    private float randomwalktimer = 0f;
    private float randomwalkchange = 0f;


	// Use this for initialization
	void Start () {
        randomwalkchange = Random.Range(2f, 3f);
        playertr = GameObject.Find("Player").GetComponent<Transform>();
        thisrb = this.GetComponent<Rigidbody>();
        thistr = this.GetComponent<Transform>();
        hivemindposition = thistr.position;
		
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
        //thisrb.AddForce((thistr.position - playertr.position).normalized * speed * Time.deltaTime, ForceMode.VelocityChange);
        goal = playertr.position;
    }

    void RandomWalk()
    {
        if ((thistr.position - goal).magnitude < 0.1f)
        {
            randomwalktimer += Time.deltaTime;
            if (randomwalktimer > 3f)
            {
                randomwalktimer = 0f;
                goal += new Vector3(Random.Range(-1.0f, 1f), 0f, Random.Range(-1.0f, 1f));
            }
        }
    }

    void MoveSelf()
    {
        thisrb.AddForce((goal - thistr.position).normalized * speed * Time.deltaTime, ForceMode.VelocityChange);
    }
	// Update is called once per frame
	void Update () {
        randomwalkmedian = hivemindposition + spawnpoint;
        SetDestinationSelf();
        Debug.Log(agro);
        Debug.Log(goal);
    }

    private void FixedUpdate()
    {
        MoveSelf();
    }
}
