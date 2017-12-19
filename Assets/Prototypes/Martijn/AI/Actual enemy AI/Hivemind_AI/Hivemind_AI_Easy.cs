using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hivemind_AI_Easy : MonoBehaviour {

    public float speed = 20f;
    public int zombies = 20;
    public float spawnradius = 0.5f;
    public float agrodistance = 20f;
    public float deagrotime = 10f;

    private float deagrotimer = 0f;
    private Vector3 zombiepositionmean;
    private Vector3 destinationself;
    private Vector3 movedirection;

    bool seeing = false;
    bool agro = false;
    bool controlezombies = false;

    public GameObject Zombie;
    public Transform playertr;
    public Rigidbody thisrb;
    private Transform thistr;
    private List<GameObject> Zombies = new List<GameObject>();
    private List<Transform> Zombiestransform = new List<Transform>();
    private float randomwalktimer = 0f;
    private Vector3 thisgoal;

    // Use this for initialization
    void Start () {
       
        thistr = this.GetComponent<Transform>();
        thisgoal = thistr.position;
        Debug.Log(thisgoal);
        for (int i = 1; i <= zombies; i++)
        {
            Vector3 spawnvector = new Vector3(Mathf.Sin(i) * i * spawnradius, 0, Mathf.Cos(i) * i * spawnradius) + this.transform.position;
            var zombie = Instantiate(Zombie, spawnvector, Quaternion.identity) as GameObject;
            zombie.GetComponent<Zombie_AI>().spawnpoint = spawnvector - thistr.position;
            zombie.GetComponent<Zombie_AI>().hivemindposition = thistr.position;
            zombie.GetComponent<Zombie_AI>().goal = spawnvector;
            zombie.GetComponent<Zombie_AI>().agro = false;
            zombie.layer = 2;
            Zombies.Add(zombie);
            Zombiestransform.Add(zombie.transform);
        }
        //playertr = GameObject.Find("Player").GetComponent<Transform>();    Werkt niet, weet niet waarom, maak ze maar even public
        //thisrb = this.GetComponent<Rigidbody>();
    }

    void CheckInLineOfSight()
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
    }

    void CheckAgro()
    {
        if (seeing && !agro)
        {
            agro = true;
            SetAgroZombies();
        }
        if(seeing && agro)
        {
            //SetDestinationZombies();
        }
        if(!seeing && agro)
        {
            deagrotimer += Time.deltaTime;
            if(deagrotimer > deagrotime)
            {
                agro = false;
                SetAgroZombies();
            }
        }
       

      

    }

    void SetAgroZombies()
    {
        if (agro)
        {
            foreach(GameObject i in Zombies)
            {
                i.GetComponent<Zombie_AI>().agro = true; 
                // Ze volgen nu automatisch de player
            }
        }
        else
        {
            foreach (GameObject i in Zombies)
            {
                i.GetComponent<Zombie_AI>().agro = false;

            }
        }
    }

    void SetDestinationSelf()
    {
        if (agro){
            zombiepositionmean = Vector3.zero;
            int count = new int();
            foreach (Transform transform in Zombiestransform)
            {
                count++;
                zombiepositionmean += transform.position;

            }
            thisgoal = zombiepositionmean / count;
        }
        else
        {
            RandomWalk();
        }
    }
    void RandomWalk()
    {
        if ((thistr.position - thisgoal).magnitude < 1f)
        {
            randomwalktimer += Time.deltaTime;
            if (randomwalktimer > 3f)
            {
                randomwalktimer = 0f;
                thisgoal += new Vector3(Random.Range(-3.0f, 3f), 0f, Random.Range(-3.0f, 3f));
            }
        }
    }
    void MoveToDestination()
    {
        movedirection = thisgoal - thistr.position;
        thisrb.AddForce(movedirection.normalized * speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    void GiveZombiesHiveMindPosition()
    {
        if (!agro) {
            foreach (GameObject i in Zombies)
            {
                i.GetComponent<Zombie_AI>().hivemindposition = thistr.position;
            }
        }
    }
// Update is called once per frame
    void Update () {
        CheckInLineOfSight();
        CheckAgro();
        SetDestinationSelf();
        GiveZombiesHiveMindPosition();
		
	}

    private void FixedUpdate()
    {
        MoveToDestination();
    }
}
