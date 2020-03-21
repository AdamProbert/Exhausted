using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleManager))]
public class Race : GameModeBase
{
    private List<Waypoint> waypoints = new List<Waypoint>();
    private List<Player> activePlayers = new List<Player>();
    private List<Player> deadPlayers = new List<Player>();
    private BattleManager battleManager;
    Player humanPlayer;
    Spawner spawner;

    Waypoint start;
    Waypoint finish;
    bool started = false;

    ////////
    // LOOK HERE: Waypoints aren't being created correcltly NO NEXT WAYPOINT ON SOME OF THEM.. apart from this. IT SHOUDLD WORK :O
    /////

    private void Awake() 
    {
        battleManager = GetComponent<BattleManager>();    
        foreach (Waypoint wp in GameObject.FindObjectsOfType<Waypoint>())
        {
            waypoints.Add(wp);
            if(wp.start)
                start = wp;
            if(wp.finish)
                finish = wp;

            if(!wp.start)
            {
                wp.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    
    public override void Setup(int numberOfEnemies, Player humanPlayer)
    {
        this.humanPlayer = humanPlayer;
        // Then collect references to everyone
        foreach(Player p in FindObjectsOfType<Player>())
        {
            activePlayers.Add(p);
            p.GetComponent<CheckpointManager>().SetTargetPoint(start);
            p.GetComponent<PlayerEventManager>().GlobalPlayerStateChange += HandlePlayerStateChange;
            p.GetComponent<PlayerEventManager>().GlobalPlayerReachedWaypoint += HandlePlayerReachedWaypoint;
        }
    }

    public void HandlePlayerReachedWaypoint(Player player, Waypoint waypoint)
    {
        if(waypoint.finish & !started)
        {
            if(player == humanPlayer)
            {
                battleManager.GameModeFinished(true);
            }
            else
            {
                battleManager.GameModeFinished(false);
            }
        }

        if(waypoint.start)
        {
            started = true;
        }

        if(player == humanPlayer)
        {
            waypoint.nextWaypoint.transform.GetChild(0).gameObject.SetActive(true);
            waypoint.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void HandlePlayerStateChange(Player player, Player.state newstate)
    {
        if(newstate == Player.state.Dead)
        {
            player.GetComponent<PlayerEventManager>().GlobalPlayerStateChange -= HandlePlayerStateChange;
            activePlayers.Remove(player);
            deadPlayers.Add(player);
            if(player == humanPlayer)
            {
                battleManager.GameModeFinished(false);
            }   
        }

        if(activePlayers.Count == 1)
        {
            Debug.Log("Human Player won the FreeForAll!");
            battleManager.GameModeFinished(true);            
        }
    }

}
