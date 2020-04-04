using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Skeleton : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (AtEndOfPath())
        {
            MoveToLocation(player.transform.position);
        }
    }

    void MoveToLocation(Vector3 destination)
    {
        agent.destination = destination;
        agent.isStopped = false;
    }

    bool AtEndOfPath()
    {
        bool end = false;
        animator.SetBool("Running_b", true);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetBool("Running_b", false);
            end = true;
        }

        return end;
    }
}
