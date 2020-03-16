using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentLocomotion : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField] bool debugAgent = false;
    [SerializeField] float debugPathTime = 1f;
    float elapsedDebugTime = 0f;


    [Header("Agent")]
    NavMeshPath path;
    NavMeshAgent agent;
    Vector3 currentDestination;

    // Agent status
    bool moving = false;

    private void Start() 
    {
        agent = GetComponent<NavMeshAgent>();    
        path = new NavMeshPath();
    }
    public void SetAgentDestination(Vector3 position)
    {
        currentDestination = position;
        agent.SetDestination(currentDestination);
    }

    private void Update() 
    {
        DebugStuff();
        AgentBeganMovingCheck();
        DestinationReachedCheck();
    }

    void AgentBeganMovingCheck()
    {
        if(!moving)
        {
            if(agent.velocity.sqrMagnitude > 0f)
            {
                CampaignEventManager.TriggerEvent("PlayerStartedMoving", null);
                moving = true;

            }
        }
    }

    void DestinationReachedCheck()
    {
        if(moving & agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    moving = false;
                    CampaignEventManager.TriggerEvent("PlayerReachedDestination", null);
                }
            }
        }
    }

    void DebugStuff()
    {
        if(debugAgent)
        {
            elapsedDebugTime += Time.deltaTime;
            if(elapsedDebugTime > debugPathTime)
            {
                elapsedDebugTime = 0f;
                NavMesh.CalculatePath(transform.position, currentDestination, NavMesh.AllAreas, path);
                for (int i = 0; i < path.corners.Length - 1; i++)
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.white, debugPathTime);
            }       
        }
    }
}
