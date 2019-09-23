using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RubbishTruckController : MonoBehaviour {

    public TileManager tileManager;

    private List<TileController> m_TilesToGoTo;
    private NavMeshAgent agent;
    private Vector3 originalSpawnPoint;

    void Awake () {
        agent = GetComponent<NavMeshAgent>();
        m_TilesToGoTo = new List<TileController>();
    }

    void Start()
    {
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(transform.position, out myNavHit, 100.0f, -1))
        {
            originalSpawnPoint = myNavHit.position;
            transform.position = originalSpawnPoint;
        }
        else
        {
            Debug.Log("Rubbish truck couldnt find nav mesh to spawn on");
        }


    }	
	// Update is called once per frame
	void Update () {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    ChooseNewTile();
                }
            }
        }
    }

    void ChooseNewTile()
    {
        if (m_TilesToGoTo.Count > 0)
        {
            int rand = Random.Range(0, m_TilesToGoTo.Count);
            Vector3 moveToLocation = m_TilesToGoTo[rand].transform.position;

            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(moveToLocation, out myNavHit, 100.0f, -1))
            {
                Debug.Log("Found Position for Rubbish Truck to go to");

                moveToLocation = myNavHit.position;
            }

            bool bSuccess = agent.SetDestination(moveToLocation);
            if (!bSuccess)
            {
                Debug.Log("Rubbish truck could not set destination");
            }
        }
    }

    public void SetTilesToGoTo(List<TileController> _tiles)
    {
        m_TilesToGoTo = _tiles;
        ChooseNewTile();
    }

}
