using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IState
{
    void OnStateEnter(Animator animController);
    void OnStateUpdate(StateMachine statMachine, Animator animController);
    void OnStateExit(Animator animController);
}

public class StateMachine
{
    IState currState;
    Animator animController;

    public bool Dead { get; set; }

    public enum States { Idle, Run, Attack, Hit, Dead }

    public States CurrentState
    {
        get
        {
            States currentState;
            if (animController.GetBool("Running_b"))
                currentState = States.Run;
            else if (animController.GetBool("Attacking_b"))
                currentState = States.Attack;
            else if (animController.GetBool("Dead_b"))
                currentState = States.Dead;
            else
                currentState = States.Idle;

            return currentState;
        }
    }

    public StateMachine(Animator anim)
    {
        Dead = false;
        animController = anim;
    }

    public void ChangeState(IState newState)
    {
        if (currState != null)
            currState.OnStateExit(animController);

        currState = newState;
        currState.OnStateEnter(animController);
    }

    public void Update()
    {
        if (currState != null && !Dead)
            currState.OnStateUpdate(this, animController);
    }
}

/// <summary>
/// Defines the Idle state
/// </summary>
public class IdleState : IState
{
    void IState.OnStateEnter(Animator animController)
    {
        animController.SetBool("Running_b", false);
        animController.SetBool("Attacking_b", false);
        animController.SetBool("Hit_b", false);
    }

    void IState.OnStateUpdate(StateMachine statMachine, Animator animController)
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

    void IState.OnStateUpdate(StateMachine statMachine, Animator animController)
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

    void IState.OnStateUpdate(StateMachine statMachine, Animator animController)
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
public class HitState : IState
{
    void IState.OnStateEnter(Animator animController)
    {
        animController.SetBool("Hit_b", true);
    }

    void IState.OnStateUpdate(StateMachine statMachine, Animator animController)
    {
        if (animController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            IState nextState = new IdleState();
            if (animController.GetBool("Running_b"))
                nextState = new RunState();
            else if (animController.GetBool("Attacking_b"))
                nextState = new AttackState();

            statMachine.ChangeState(nextState);
        }
    }
    void IState.OnStateExit(Animator animController)
    {
        animController.SetBool("Hit_b", false);
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

    void IState.OnStateUpdate(StateMachine statMachine, Animator animController)
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
    [SerializeField]
    Melee melee;

    private NavMeshAgent agent;
    private Animator animator;
    GameObject player;
    StateMachine stateMachine;

    float deadTimer = 10;
    float pauseTimer = 3;
    int disapearSpeed = 1;
    bool dead = false;
    bool ambush = true;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        stateMachine = new StateMachine(animator);
        stateMachine.ChangeState(new IdleState());
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();

        if (ambush)
            return;

        if (agent && AtEndOfPath() && !dead)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
            {
                melee.Strike = true;
                stateMachine.ChangeState(new AttackState());
            }
            else
            {
                melee.Strike = false;
                stateMachine.ChangeState(new RunState());
                MoveToLocation(player.transform.position);
            }
        }

        if (!dead && stateMachine.CurrentState == StateMachine.States.Attack)
        {
            dead = true;
            StartCoroutine(PauseTimer());
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
        while (!complete)
        {
            transform.position += Vector3.down * disapearSpeed * Time.deltaTime;

            complete = transform.position.y <= -5;
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    IEnumerator PauseTimer()
    {
        yield return new WaitForSeconds(pauseTimer);

        dead = false;
        stateMachine.ChangeState(new RunState());
    }

    /// <summary>
    /// Registers a hit if the collider doesn't pick it up
    /// </summary> 
    public void Hit()
    {
        dead = true;
        stateMachine.Dead = true;
        stateMachine.ChangeState(new DeadState());
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
            stateMachine.ChangeState(new DeadState());
            agent.isStopped = true;
            Destroy(agent);
            StartCoroutine(DeadTimer());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ambush = false;
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
