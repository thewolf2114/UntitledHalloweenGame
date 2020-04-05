using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IState
{
    void OnStateEnter(Animator animController);
    void OnStateUpdate();
    void OnStateExit(Animator animController);
}

public class StateMachine
{
    IState currState;

    public bool Dead { get; set; }

    public StateMachine()
    {
        Dead = false;
    }

    public void ChangeState(Animator animController, IState newState)
    {
        if (currState != null)
            currState.OnStateExit(animController);

        currState = newState;
        currState.OnStateEnter(animController);
    }

    public void Update()
    {
        if (currState != null && !Dead)
            currState.OnStateUpdate();
    }
}

/// <summary>
/// Defines the Idle state
/// </summary>
public class IdleState : IState
{
    void IState.OnStateEnter(Animator animController)
    {
        
    }

    void IState.OnStateUpdate()
    {

    }
    void IState.OnStateExit(Animator animController)
    {
        
    }
}

/// <summary>
/// Defines the Run state
/// </summary>
public class RunState : IState
{
    void IState.OnStateEnter(Animator animController)
    {
        animController.SetBool("Running_b", true);
    }

    void IState.OnStateUpdate()
    {

    }
    void IState.OnStateExit(Animator animController)
    {
        animController.SetBool("Running_b", false);
    }
}

/// <summary>
/// Defines the Attack state
/// </summary>
public class AttackState : IState
{
    void IState.OnStateEnter(Animator animController)
    {
        animController.SetBool("Attacking_b", true);
    }

    void IState.OnStateUpdate()
    {

    }
    void IState.OnStateExit(Animator animController)
    {
        animController.SetBool("Attacking_b", false);
    }
}

/// <summary>
/// Defines the Dead state
/// </summary>
public class DeadState : IState
{
    void IState.OnStateEnter(Animator animController)
    {
        animController.SetBool("Dead_b", true);
    }

    void IState.OnStateUpdate()
    {

    }
    void IState.OnStateExit(Animator animController)
    {
        
    }
}

//[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class Skeleton : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    GameObject player;
    StateMachine stateMachine = new StateMachine();

    float deadTimer = 10;
    int disapearSpeed = 1;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        stateMachine.ChangeState(animator, new IdleState());
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();

        if (agent && AtEndOfPath() && !dead)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
            {
                stateMachine.ChangeState(animator, new AttackState());
            }
            else
            {
                stateMachine.ChangeState(animator, new RunState());
                MoveToLocation(player.transform.position);
            }
        }
    }

    /// <summary>
    /// Move the agent to a desired location on the navmesh
    /// </summary>
    /// <param name="destination">the point in world space you want to go</param>
    void MoveToLocation(Vector3 destination)
    {
        agent.destination = destination;
        agent.isStopped = false;
    }

    bool AtEndOfPath()
    {
        bool end = false;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            end = true;
        }

        return end;
    }

    /// <summary>
    /// Starts a timer to wait and then start the kill sequence
    /// </summary>
    /// <returns></returns>
    IEnumerator DeadTimer()
    {
        yield return new WaitForSeconds(deadTimer);

        StartCoroutine(KillSequence());
    }

    /// <summary>
    /// Pushes the skeleton through the floor and when it reaches a certain depth
    /// destroy it.
    /// </summary>
    /// <returns></returns>
    IEnumerator KillSequence()
    {
        bool complete = false;
        Vector3 position;
        while (!complete)
        {
            transform.position += Vector3.down * disapearSpeed * Time.deltaTime;

            complete = transform.position.y <= -5;
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Registers a hit if the collider doesn't pick it up
    /// </summary> 
    public void Hit()
    {
        dead = true;
        stateMachine.Dead = true;
        stateMachine.ChangeState(animator, new DeadState());
        agent.isStopped = true;
        Destroy(agent);
        StartCoroutine(DeadTimer());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile") && !dead)
        {
            dead = true;
            stateMachine.Dead = true;
            stateMachine.ChangeState(animator, new DeadState());
            agent.isStopped = true;
            Destroy(agent);
            StartCoroutine(DeadTimer());
        }
    }
}
