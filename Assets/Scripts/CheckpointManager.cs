using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] LayerMask checkpointLayer;
    private Waypoint targetPoint;
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
        if(targetPoint)
        {
            if(checkpointLayer == (checkpointLayer | (1 << other.gameObject.layer)) & other.gameObject == targetPoint.gameObject)
            {
                playerEventManager.GlobalPlayerReachedWaypoint(GetComponent<Player>(), other.GetComponent<Waypoint>());
                SetTargetPoint(other.GetComponent<Waypoint>().nextWaypoint.GetComponent<Waypoint>());
            }
        }
    }    
}
