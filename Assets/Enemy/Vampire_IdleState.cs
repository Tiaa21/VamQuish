using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vampire_IdleState : StateMachineBehaviour
{

    float timer;
    public float idleTime = 0f;

    Transform player;

    public float detectionAreaRadius = 18f;

    //
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Moving to the patrolling
        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            animator.SetBool("isPatrolling", true);
        }

        //Moving to the chasing
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }

}
