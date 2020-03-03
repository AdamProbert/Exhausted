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
    }

    void DebugStuff()
    {
        if(debugAgent)
        {
            elapsedDebugTime += Time.deltaTime;
            if(elapsedDebugTime > debugPathTime)
            {
                Debug.Log("Should be drawing");
                elapsedDebugTime = 0f;
                NavMesh.CalculatePath(transform.position, currentDestination, NavMesh.AllAreas, path);
                for (int i = 0; i < path.corners.Length - 1; i++)
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.white, debugPathTime);
            }       
        }
    }
}
