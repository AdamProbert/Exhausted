using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleManager))]
[RequireComponent(typeof(AudioSource))]
public class Race : GameModeBase
{
    [SerializeField] AudioClip checkpointSound;
    private List<Waypoint> waypoints = new List<Waypoint>();
    private List<Player> activePlayers = new List<Player>();
    private List<Player> deadPlayers = new List<Player>();
    private BattleManager battleManager;
    private AudioSource audioSource;
    Player humanPlayer;
    Spawner spawner;

    Waypoint start;
    Waypoint finish;
    bool started = false;

    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
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

    public override void Setup(int numberOfEnemies)
    {
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

        if(waypoint.finish & started)
        {
            if(humanPlayer != null && player == humanPlayer)
            {
                battleManager.GameModeFinished(true);
            }
            else if(humanPlayer != null && player != humanPlayer)
            {
                battleManager.GameModeFinished(false);
            }
            else
            {
                Debug.Log("Player " + player.name + " won the race");
                battleManager.GameModeFinished(true);
            }
        }

        if(waypoint.start & !started)
        {
            started = true;
        }        

        if(humanPlayer != null & player == humanPlayer)
        {
            audioSource.PlayOneShot(checkpointSound, 1f);
            waypoint.nextWaypoint.EnableVisuals();
            waypoint.DisableVisuals();
        }
    }

    public void HandlePlayerStateChange(Player player, Player.state newstate)
    {
        if(newstate == Player.state.Dead)
        {
            player.GetComponent<PlayerEventManager>().GlobalPlayerStateChange -= HandlePlayerStateChange;
            activePlayers.Remove(player);
            deadPlayers.Add(player);
            if(humanPlayer != null && player == humanPlayer)
            {
                battleManager.GameModeFinished(false);
            }   
        }

        if(activePlayers.Count == 1)
        {
            Debug.Log(activePlayers[0].name + " won the race! (By killing everone else)");
            battleManager.GameModeFinished(true);            
        }
    }
}
