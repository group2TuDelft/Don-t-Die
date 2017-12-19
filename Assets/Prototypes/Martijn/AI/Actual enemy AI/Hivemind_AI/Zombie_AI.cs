using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_AI : MonoBehaviour {

    public Vector3 goal { get; set; }
    public bool agro { get; set; }
    private Vector3 destination;
    public float speed = 20f;
    public Vector3 hivemindposition { get; set; }
    public Vector3 spawnpoint;

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
        thisrb.constraints = RigidbodyConstraints.FreezeRotationX;
      
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
        if (((goal - thistr.position).magnitude < 1f) || ((goal - thistr.position).magnitude > 5f))
        {
            randomwalktimer += Time.deltaTime;
            if (randomwalktimer > 3f)
            {
                randomwalkmedian = hivemindposition + spawnpoint;
                randomwalktimer = 0f;
                goal = randomwalkmedian + new Vector3(Random.Range(-3.0f, 3.0f), 0f, Random.Range(-3.0f, 3.0f));
            }
        }
    }

    void SetPath()
    {
        RaycastHit hitInfo;
        Vector3 direction = playertr.position - thistr.position;
        if (Physics.Raycast(thistr.position, direction, out hitInfo))
        {
            if(hitInfo.collider.tag == "Player")
            {
                destination = playertr.position;
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
        thistr.forward = (goal - thistr.position);
    }
	// Update is called once per frame
	void Update () {
        SetDestinationSelf();
        SetPath();
    }

    private void FixedUpdate()
    {
        MoveSelf();
    }
}
