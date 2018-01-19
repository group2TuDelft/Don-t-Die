using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolingEnemy : MonoBehaviour {
    public Transform playertr;
    public Transform enemytr;
    public NavMeshAgent navMesh;

   
    public float agrodistance = 100f;
    public float agroangle = 30f;
    public float waypointtollerance = 10f;

    private float timer = 0f;

    private bool chasing = false;
    private bool seeing = false;

    private int currentwaypointindex = 0;

    private int amountofwaypoints = 7;
    public Transform wp1;
    public Transform wp2;
    public Transform wp3;
    public Transform wp4;
    public Transform wp5;
    public Transform wp6;
    public Transform wp7;

    List<Vector3> wplist = new List<Vector3>(); // Maak een lijst met de waypoints zodat ik straks met de index de waypoint kan veranderen van de enemy


    // Use this for initialization
    void Start () {
        //navMesh = GetComponent<NavMeshAgent>();
        navMesh.SetDestination(wp1.position);
        wplist.Add(wp1.position);
        wplist.Add(wp2.position);
        wplist.Add(wp3.position);
        wplist.Add(wp4.position);
        wplist.Add(wp5.position);
        wplist.Add(wp6.position);
        wplist.Add(wp7.position);

    }

    void CheckInLineOfSight()
    {
        RaycastHit hitInfo;
        Vector3 direction = playertr.position - enemytr.transform.position;
        if (Physics.Raycast(enemytr.transform.position, direction, out hitInfo, agrodistance)) // nog angle erin zetten
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
	void Update () {
        CheckInLineOfSight(); // Kijken of enemy de player ziet
        if (seeing && !chasing){ // Begin met volgen
            Debug.Log("Wow ik zie je, volgen!");
            
            timer = 0f;
            chasing = true;
            navMesh.SetDestination(playertr.position);
           

        }
        if (chasing && !seeing) // Enemy ziet hem niet meer, loop naar de laatste locatie en kijk daar, als je langer dan x seconde weg bent ga verder patrolen
        {
            Debug.Log("Waar ben je?");
            timer += Time.deltaTime;
            Debug.Log(timer);
            if((navMesh.destination - enemytr.position).magnitude < waypointtollerance && timer > 5f)
            {
                chasing = false;
                Debug.Log("Ik ga wel weer patrolen");

            }
        }
        if (chasing && seeing) // Enemy ziet de speler, volg hem
        {
            Debug.Log("Muahaha come here");
            timer = 0f;
            navMesh.SetDestination(playertr.position);
        }
		if (!chasing && !seeing) // Als enemy player niet ziet of volgt, ga lekker patrolen
        {
            Debug.Log("Ladida, alles prima");
            Debug.Log(wplist);
            if ((wplist[currentwaypointindex] - enemytr.position).magnitude < waypointtollerance)
            {
                Debug.Log("Volgende waypoint");
                if (currentwaypointindex == 6)
                {
                    currentwaypointindex = 0;
                }
                else
                {
                    currentwaypointindex++;
                }
            }
            navMesh.SetDestination(wplist[currentwaypointindex]);
        }   
	}
}
