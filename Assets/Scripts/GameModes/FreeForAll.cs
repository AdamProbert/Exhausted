using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
[RequireComponent(typeof(BattleManager))]
public class FreeForAll : GameModeBase
{

    private List<Player> activePlayers = new List<Player>();
    private List<Player> deadPlayers = new List<Player>();
    private BattleManager battleManager;
    Player humanPlayer;
    Spawner spawner;
    private IEnumerator startGameRoutine;

    private void Awake() 
    {
        spawner = GetComponent<Spawner>();
        battleManager = GetComponent<BattleManager>();    
        base.type = GameModeType.FreeForAll;
        startGameRoutine = StartGame();
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

    public override void StartItUp(bool wholeSequence)
    {
        if(wholeSequence)
        {
            StartCoroutine(startGameRoutine);
        }
        else
        {
            GameManager.Instance.SetGameState(GameManager.GameState.PLAY);
            // Activate weapons
            foreach (Player p in activePlayers)
            {
                p.GetComponent<PlayerEventManager>().OnWeaponStatusChange(BaseWeapon.status.Active);
            }
        }
    }


    private IEnumerator StartGame() 
    {
        if(humanPlayer != null)
        {
            CinemachineFreeLook playerCam = humanPlayer.GetComponentInChildren<CinemachineFreeLook>();
            List<SpawnPoint> spawnPoints = spawner.GetAllSpawnPoints(type);
            List<CinemachineVirtualCamera> cams = new List<CinemachineVirtualCamera>();

            foreach (SpawnPoint spawn in spawnPoints)
            {
                if(spawn.occupied != null)
                {
                    cams.Add(spawn.virtualCamera);
                }
                spawn.virtualCamera.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(1);
            playerCam.gameObject.SetActive(false);

            foreach (CinemachineVirtualCamera cam in cams)
            {
                cam.gameObject.SetActive(true);
                yield return new WaitForSeconds(timePerCinemachineMove);
                cam.gameObject.SetActive(false);    
            }

            playerCam.gameObject.SetActive(true);
            yield return new WaitForSeconds(timePerCinemachineMove);
        }
        
        GameManager.Instance.SetGameState(GameManager.GameState.PLAY);
        // Activate weapons
        foreach (Player p in activePlayers)
        {
            p.GetComponent<PlayerEventManager>().OnWeaponStatusChange(BaseWeapon.status.Active);
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
