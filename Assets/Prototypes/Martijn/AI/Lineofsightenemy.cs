using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lineofsightenemy : MonoBehaviour {
    public Transform playertr;
    private NavMeshAgent navMesh;

    public float speed = 10f;
    public float agrodistance = 100f;

	// Use this for initialization
	void Start () {
        navMesh = GetComponent<NavMeshAgent>();
    }

    
        // Update is called once per frame
        void Update () {
        //Vector3 direction = (playertr.position - enemytr.position);
        Vector3 direction = playertr.position - this.transform.position;
        RaycastHit hitInfo;
        if (Physics.Raycast(this.transform.position, direction, out hitInfo, agrodistance))
        {
            if (hitInfo.collider.tag == "Player")
            {
                navMesh.SetDestination(playertr.position);
            }
        }
  
        else
            navMesh.SetDestination(this.transform.position);
            

        

    
		
	}
}
