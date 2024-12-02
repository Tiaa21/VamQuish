using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navAgent;
    
    private void Start()
    {
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
    
            //Check if the ray or the position of the cursor hits the ground
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, UnityEngine.AI.NavMesh.AllAreas))
            {
                //Move agent to the spot
                navAgent.SetDestination(hit.point);
            }
        }
    }
}
