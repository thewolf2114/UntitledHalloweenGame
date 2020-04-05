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

public class Idle : IState
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

public class Run : IState
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

public class Attack : IState
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

public class Dead : IState
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
//[RequireComponent(typeof(Animator))]

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
        stateMachine.ChangeState(animator, new Idle());
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();

        if (agent && AtEndOfPath() && !dead)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
            {
                stateMachine.ChangeState(animator, new Attack());
            }
            else
            {
                stateMachine.ChangeState(animator, new Run());
                MoveToLocation(player.transform.position);
            }
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

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            end = true;
        }

        return end;
    }

    IEnumerator DeadTimer()
    {
        yield return new WaitForSeconds(deadTimer);

        StartCoroutine(KillSequence());
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            dead = true;
            stateMachine.Dead = true;
            stateMachine.ChangeState(animator, new Dead());
            agent.isStopped = true;
            Destroy(agent);
            StartCoroutine(DeadTimer());
        }
    }
}
