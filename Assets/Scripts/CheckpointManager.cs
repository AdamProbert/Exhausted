using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] LayerMask checkpointLayer;
    public Waypoint targetPoint;
    private PlayerEventManager playerEventManager;

    private void OnEnable()
    {
        playerEventManager = transform.root.GetComponent<PlayerEventManager>();
    }

    public void SetTargetPoint(Waypoint point)
    {
        targetPoint = point;
    }

    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Chekpoint manager: trigger enter");
        if(targetPoint !=null)
        {
            if(other.gameObject == targetPoint.gameObject)
            {
                playerEventManager.GlobalPlayerReachedWaypoint(GetComponent<Player>(), other.GetComponent<Waypoint>());
                SetTargetPoint(other.GetComponent<Waypoint>().nextWaypoint.GetComponent<Waypoint>());
            }
        }
    }    
}
