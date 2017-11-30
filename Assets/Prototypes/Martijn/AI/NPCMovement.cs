using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour {
    [SerializeField]
    Transform goal;
    [SerializeField] NavMeshAgent navMesh;


	// Use this for initialization
	void Start () {
        navMesh = this.GetComponent<NavMeshAgent>();
        SetDestination();
	}
	
	private void SetDestination()
    {
        Vector3 targetvector = goal.transform.position;
        navMesh.SetDestination(targetvector);
    }
    private void Update()
    {
        Vector3 targetvector = goal.transform.position;
        navMesh.SetDestination(targetvector);
    }

}
