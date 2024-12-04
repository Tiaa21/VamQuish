using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

    private NavMeshAgent navAgent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {

            int randomValue = Random.Range(0, 2); //0 or 1  

            if (randomValue == 0)
            {
                animator.SetTrigger("Die_1");
            }
            else
            {
                animator.SetTrigger("Die_2");
            }


        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f); //Attacking

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f); //Chasing

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f); //Stop Chasing
    }
}
