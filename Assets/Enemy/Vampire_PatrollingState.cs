using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vampire_PatrollingState : StateMachineBehaviour
{
    float timer;
    public float patrollingTime = 10f;

    Transform player;
    NavMeshAgent agent;

    public float detectionArea = 18f;
    public float patrolSpeed = 2f;

    List<Transform> waypointList = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = patrolSpeed;
        timer = 0;

        //Move to the first waypoint
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("WayPoints");
        foreach (Transform t in waypointCluster.transform)
        {
            waypointList.Add(t);
        }

        Vector3 nextPosition = waypointList[Random.Range(0, waypointList.Count)].position;
        agent.SetDestination(nextPosition);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Check if the enemy is at the waypoint, move to the next waypoint
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypointList[Random.Range(0, waypointList.Count)].position);
        }

        //ransition to idle state
        timer += Time.deltaTime;
        if (timer > patrollingTime)
        {
            animator.SetBool("isPatrolling", true);
        }

        //ransition to chasing state
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
}
