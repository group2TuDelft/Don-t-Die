using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hivemind_AI_Easy : MonoBehaviour {

    // Hij moet nog taunten en random ass geluiden maken

    public int scoreValue = 10; // hoe veel punten is die waard
    public int starting_health = 200; // Hoe veel hp heeft deze enemy
    public int current_health = 1;
    public float speed = 20f; // Hoe snel is die 
    public int minionsammount = 20; // Hoe veel minions spawnt die
    public float spawnradius = 0.5f;
    public float agrodistance = 20f;  // Afstand waarbij hij player niet meer wilt volgen
    public float deagrotime = 5f; // Tijd waarna agro verloren wordt
    public float minionhidedistance = 5f; // Hoe ver die achter de minions wilt hiden in agro
    private float rotationspeed = 4f; // Hoe snel die kan draaien in agro mode
    private float deagrotimer = 0f; // Timer voor deagro
    public float despawntime = 1.5f; //Tijd na dood waarna character verdwijnt
    private float summontimer = 0f;
    public float summoncooldown = 8f; //Tijd waarna hivemind een nieuwe minion summond
    public int summonammount = 4; //Hoeveel minions die summont per summon ronde

    private Vector3 minionpositionmean; // Wordt gebruikt om gemiddelde positite van de minions te vinden
    private Vector3 movedirection; // Richting waarin rg addforce wordt gedaan
    private Vector3 minionhidedirection; // Richting waar in die achter minion mean wilt staat wilt minions tussen player en hivemind hebben

    public bool seeing = false;
    public bool agro = false;
    public bool isdead = false;

    public List<GameObject> Minionslist;
    public List<Transform> Minionstransform;

    public GameObject Minion;
    public Transform playertr;
    private Rigidbody thisrb;
    private Transform thistr;
    private Animator animator;
    public AudioManager audiomanager;

    private float randomwalktimer = 0f;
    private Vector3 thisgoal; // De plek waar deze enemy naartoe wilt gaan

    // Use this for initialization
    void Start () {

        //List<GameObject> Minionslist = new List<GameObject>();
        //List<Transform> Minionstransform = new List<Transform>();
        current_health = starting_health;
        thistr = this.GetComponent<Transform>();
        thisrb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        audiomanager = FindObjectOfType<AudioManager>();
        playertr = GameObject.Find("Player").GetComponent<Transform>();
        thisgoal = thistr.position;

        for (int i = 1; i <= minionsammount; i++)
        {
            Vector3 spawnvector = new Vector3(Mathf.Sin(i) * i * spawnradius, 0, Mathf.Cos(i) * i * spawnradius) + this.transform.position;
            Summon(spawnvector);
        }
        //playertr = GameObject.Find("Player").GetComponent<Transform>();    Werkt niet, weet niet waarom, maak ze maar even public
        
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
        else
        {
            seeing = false;
        }
    }

    void CheckAgro()
    {
        if (seeing && !agro)
        {
            agro = true;
            SetAgroMinions();
        }
        if(seeing && agro)
        {
            //SetDestinationMinions();
        }
        if(!seeing && agro)
        {
            deagrotimer += Time.deltaTime;
            if(deagrotimer > deagrotime)
            {
                agro = false;
                SetAgroMinions();
            }
        }
       

      

    }

    public void SetAgroMinions()
    {
        if (agro) //Hier er nog bijzetten && Minion agro != True
        {
            foreach(GameObject i in Minionslist)
            {
                i.GetComponent<Minion_AI>().agro = true; 
                // Ze volgen nu automatisch de player
            }
        }
        else
        {
            foreach (GameObject i in Minionslist)
            {
                i.GetComponent<Minion_AI>().agro = false;
                i.GetComponent<Minion_AI>().goal = i.GetComponent<Minion_AI>().spawnpoint + thistr.position;

            }
        }
    }

    void SetDestinationSelf()
    {
        if (agro){
            FaceTarget(playertr.position);
            minionpositionmean = Vector3.zero;
            int count = new int();
            foreach (Transform transform in Minionstransform)
            {
                count++;
                minionpositionmean += transform.position;

            }
            minionpositionmean = minionpositionmean / count;
            minionhidedirection = (thistr.position - playertr.position).normalized;
            thisgoal = minionpositionmean + minionhidedirection * minionhidedistance;
            
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

    void GiveMinionsHiveMindPosition()
    {
        if (!agro) {
            foreach (GameObject i in Minionslist)
            {
                i.GetComponent<Minion_AI>().hivemindposition = thistr.position;
            }
        }
    }
// Update is called once per frame
    void Update () {
        CheckInLineOfSight();
        CheckAgro();
        CheckSummon();
        SetDestinationSelf();
        GiveMinionsHiveMindPosition();
		
	}

    private void FixedUpdate()
    {
        MoveToDestination();
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 lookPos = target - thistr.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationspeed);
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
            audiomanager.RandomPlay("HivemindDeath");
            Destroy(this.gameObject, despawntime);
        }
        isdead = true;
        thisgoal = thistr.position;


        //capsuleCollider.isTrigger = true;

        //ScoreManager.score += scoreValue;

    }
    void CheckSummon()
    {
        if (agro)
        {
            summontimer += Time.deltaTime;
            if (summontimer > summoncooldown)
            {
                animator.SetBool("AnimSpawn", true);
                audiomanager.RandomPlay("HivemindTaunt");
                summontimer = 0f;
                Vector3 firstsummon = new Vector3();
                for (int i = 1; i == summonammount; i++)
                {

                    if (i == 1)
                    {
                        firstsummon = (thistr.position - playertr.position).normalized * 4;// Afstans van hivemind dat die spawned
                        Summon(firstsummon);
                    }
                    else
                    {
                        Vector3 spawnvector = firstsummon + new Vector3(Random.Range(2f, 4f), 0, Random.Range(2f, 4f));
                        //Vector3 spawnvector = (Vector3.Cross((firstsummon - thistr.position), Vector3.up) * i) * (-1 ^ i);
                        Summon(spawnvector);
                    }
                
                }
            
            }
            else
            {
                animator.SetBool("AnimSpawn", false);
            }
        }
        else
        {
            animator.SetBool("AnimSpawn", false);
        }
        
    }
    void Summon(Vector3 spawnvector)
    {
        GameObject minion = Instantiate(Minion, spawnvector, Quaternion.identity);
        minion.GetComponent<Minion_AI>().spawnpoint = spawnvector - thistr.position;
        minion.GetComponent<Minion_AI>().hivemindposition = thistr.position;
        minion.GetComponent<Minion_AI>().goal = spawnvector;
        minion.GetComponent<Minion_AI>().agro = false;
        minion.layer = 2;
        Minionslist.Add(minion);
        Minionstransform.Add(minion.transform);
    }
}   

