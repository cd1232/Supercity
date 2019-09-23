using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Train : MonoBehaviour {

    public Transform goal;
    private Vector3 start;
    private NavMeshAgent agent;
    private int destinationCount = 0;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start()
    {
        NavMeshHit hit;
        Vector3 newGoal;
        if (NavMesh.SamplePosition(goal.position, out hit, 40.0f, NavMesh.GetAreaFromName("Train")))
        {
            newGoal = hit.position;
            goal.position = newGoal;
        }

        if (NavMesh.SamplePosition(transform.position, out hit, 40.0f, NavMesh.GetAreaFromName("Train")))
        {
            transform.position = hit.position;
            start = transform.position;
        }

        GetComponent<NavMeshAgent>().SetDestination(goal.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    PathEnd();
                }
            }
        }
    }

    void PathEnd()
    {
        transform.Rotate(0, 180.0f, 0);
        if (destinationCount == 0)
        {
            GetComponent<NavMeshAgent>().SetDestination(start);
            destinationCount = 1;
        }
        else
        {
            GetComponent<NavMeshAgent>().SetDestination(goal.position);
            destinationCount = 0;
        }
    }
}
