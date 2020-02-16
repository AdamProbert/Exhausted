using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class BattleManager : MonoBehaviour
{
    
    private List<Player> activePlayers = new List<Player>();
    private List<Player> deadPlayers = new List<Player>();

    private Player humanPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(Player p in FindObjectsOfType<Player>())
        {
            if(!p.isAI)
            {
                humanPlayer = p;
            }
            activePlayers.Add(p);
            p.OnStateChange += HandlePlayerDied;
        }
    }

    public void HandlePlayerDied(Player.playerState newstate, Player deadPlayer)
    {
        if(newstate == Player.playerState.Dead)
        {
            activePlayers.Remove(deadPlayer);
            deadPlayers.Add(deadPlayer);   
        }
        if(activePlayers.Count == 1)
        {
            Debug.Log("One player left! " + activePlayers[0].name + " WON!");
            EndGame();
        }
        if(deadPlayer == humanPlayer)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
