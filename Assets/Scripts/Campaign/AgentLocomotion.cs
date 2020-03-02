using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentLocomotion : MonoBehaviour
{

    NavMeshAgent agent;

    private void Start() 
    {
        agent = GetComponent<NavMeshAgent>();    
    }
    public void SetAgentDestination(Vector3 position)
    {
        agent.SetDestination(position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
