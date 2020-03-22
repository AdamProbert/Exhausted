using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleManager))]
public class FreeForAll : GameModeBase
{

    private List<Player> activePlayers = new List<Player>();
    private List<Player> deadPlayers = new List<Player>();
    private BattleManager battleManager;
    Player humanPlayer;
    Spawner spawner;

    private void Awake() 
    {
        battleManager = GetComponent<BattleManager>();    
        base.type = GameModeType.FreeForAll;
    }
    
    public override void Setup(int numberOfEnemies, Player humanPlayer)
    {
        this.humanPlayer = humanPlayer;
        // Then collect references to everyone
        foreach(Player p in FindObjectsOfType<Player>())
        {
            activePlayers.Add(p);
            p.GetComponent<PlayerEventManager>().GlobalPlayerStateChange += HandlePlayerStateChange;
        }
    }

    public override void Setup(int numberOfEnemies)
    {
        // Then collect references to everyone
        foreach(Player p in FindObjectsOfType<Player>())
        {
            activePlayers.Add(p);
            p.GetComponent<PlayerEventManager>().GlobalPlayerStateChange += HandlePlayerStateChange;
        }
    }

    public void HandlePlayerStateChange(Player player, Player.state newstate)
    {
        if(newstate == Player.state.Dead)
        {
            player.GetComponent<PlayerEventManager>().GlobalPlayerStateChange -= HandlePlayerStateChange;
            activePlayers.Remove(player);
            deadPlayers.Add(player);

            if(humanPlayer != null & player == humanPlayer)
            {
                battleManager.GameModeFinished(false);
            }   
        }

        if(activePlayers.Count == 1)
        {
            Debug.Log(activePlayers[0].name + " won the FreeForAll!");
            battleManager.GameModeFinished(true);            
        }
    }

}
