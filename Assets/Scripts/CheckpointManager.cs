using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] LayerMask checkpointLayer;
    public Waypoint targetPoint;
    private PlayerEventManager playerEventManager;
    public int currentLap;    
    private int racePostion;

    private void OnEnable()
    {
        playerEventManager = transform.root.GetComponent<PlayerEventManager>();
    }

    public void SetTargetPoint(Waypoint point)
    {   
        // First point
        if(targetPoint == null)
        {
            playerEventManager.HandleWaypointReachedForAI(GetComponent<Player>(), point);
        }

        targetPoint = point;
    }

    public void SetRacePosition(int pos, int playerpos)
    {
        if(racePostion != pos)
        {
            racePostion = pos;
            playerEventManager.GlobalRacePositionChanged(GetComponent<Player>(), racePostion);
        }
        
        if(pos > playerpos)
        {
            playerEventManager.OnRaceBehindPlayer();
        }
        else if(pos < playerpos)
        {
            playerEventManager.OnRaceAheadOfPlayer();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(targetPoint != null)
        {
            if(other.gameObject == targetPoint.gameObject)
            {
                SetTargetPoint(other.GetComponent<Waypoint>().nextWaypoint.GetComponent<Waypoint>());
                playerEventManager.GlobalPlayerReachedWaypoint(GetComponent<Player>(), other.GetComponent<Waypoint>());
            }
        }
    }    
}
