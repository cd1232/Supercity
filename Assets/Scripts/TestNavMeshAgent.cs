using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavMeshAgent : MonoBehaviour {

    public Transform goal;

	// Use this for initialization
	void Start () {
        NavMeshHit hit;
        Vector3 newGoal;
        if (NavMesh.SamplePosition(goal.position, out hit, 20.0f, NavMesh.GetAreaFromName("Train")))
        {
            newGoal = hit.position;
        }

        if (NavMesh.SamplePosition(transform.position, out hit, 20.0f, NavMesh.GetAreaFromName("Train")))
        {
            transform.position = hit.position;
        }

        GetComponent<NavMeshAgent>().SetDestination(goal.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
