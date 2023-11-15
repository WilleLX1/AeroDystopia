using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StalkerEnemy : MonoBehaviour
{
    public GameObject player;
    public bool isPlayerLooking = false;
    public float speed = 10.0f;
    public Transform[] waypoints;
    private int destPoint = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GotoNextPoint();
    }
    
    void GotoNextPoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.destination = waypoints[destPoint].position;
        destPoint = (destPoint + 1) % waypoints.Length;
    }

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No GameObject found with the Player tag");
            return;
        }

        // Check players raycast that is forward
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, 100))
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                isPlayerLooking = true;
            }
            else 
            {
                isPlayerLooking = false;
            }
        }

        if (!isPlayerLooking)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
        else
        {
            agent.destination = player.transform.position;
        }
    }
}
