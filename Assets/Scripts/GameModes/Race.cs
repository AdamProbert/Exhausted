using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using TMPro;

[RequireComponent(typeof(BattleManager))]
[RequireComponent(typeof(AudioSource))]
public class Race : GameModeBase
{   
    [SerializeField] GameObject raceui;
    [SerializeField] TextMeshProUGUI racePositionText;

    [SerializeField] AudioClip checkpointSound;
    [SerializeField] int totalLapCount;
    private List<Waypoint> waypoints = new List<Waypoint>();
    private List<Player> activePlayers = new List<Player>();
    private List<Player> deadPlayers = new List<Player>();
    private BattleManager battleManager;
    private AudioSource audioSource;

    private static Dictionary<Player, int> lapCounter;
    private static Dictionary<Player, int> checkpointCounter; 

    private List<Player> racePositions;
    Player humanPlayer;
    Spawner spawner;

    Waypoint start;
    Waypoint finish;
    private IEnumerator startGameRoutine;
    [Header("Camera stuff")]

    [SerializeField] float weaponFreezeTime = 5f;

    private void Awake() 
    {
        lapCounter = new Dictionary<Player, int>();
        checkpointCounter = new Dictionary<Player, int>();
        spawner = GetComponent<Spawner>();
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
        startGameRoutine = StartGame();
    }
    
    public override void Setup(int numberOfEnemies, Player humanPlayer)
    {
        raceui.SetActive(true);
        this.humanPlayer = humanPlayer;
        // Then collect references to everyone
        foreach(Player p in FindObjectsOfType<Player>())
        {
            activePlayers.Add(p);
            p.GetComponent<CheckpointManager>().SetTargetPoint(start);
            p.GetComponent<PlayerEventManager>().GlobalPlayerStateChange += HandlePlayerStateChange;
            p.GetComponent<PlayerEventManager>().GlobalPlayerReachedWaypoint += HandlePlayerReachedWaypoint;
            lapCounter.Add(p, -1);
            checkpointCounter.Add(p,-1);
        }
    }

    
    public override void Setup(int numberOfEnemies)
    {
        raceui.SetActive(true);
        // Then collect references to everyone
        foreach(Player p in FindObjectsOfType<Player>())
        {
            activePlayers.Add(p);
            p.GetComponent<CheckpointManager>().SetTargetPoint(start);
            p.GetComponent<PlayerEventManager>().GlobalPlayerStateChange += HandlePlayerStateChange;
            p.GetComponent<PlayerEventManager>().GlobalPlayerReachedWaypoint += HandlePlayerReachedWaypoint;
            lapCounter.Add(p, -1);
            checkpointCounter.Add(p,-1);
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
        // Freeze Weapons
        foreach (Player p in activePlayers)
        {
            p.GetComponent<PlayerEventManager>().OnWeaponStatusChange(BaseWeapon.status.Inactive);
        }

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
                yield return new WaitForSeconds(base.timePerCinemachineMove);
                cam.gameObject.SetActive(false);    
            }

            playerCam.gameObject.SetActive(true);
            yield return new WaitForSeconds(base.timePerCinemachineMove);
        }
        
        GameManager.Instance.SetGameState(GameManager.GameState.PLAY);

        yield return new WaitForSeconds(weaponFreezeTime);

        // Activate weapons
        foreach (Player p in activePlayers)
        {
            p.GetComponent<PlayerEventManager>().OnWeaponStatusChange(BaseWeapon.status.Active);
        }
    }

    public void HandlePlayerReachedWaypoint(Player player, Waypoint waypoint)
    {
        checkpointCounter[player] += 1;

        if(waypoint.finish && lapCounter[player] != -1)
        {
            lapCounter[player] += 1;
            player.GetComponent<CheckpointManager>().currentLap = lapCounter[player];
            
            if(lapCounter[player] >= totalLapCount)
            {
                if(humanPlayer != null && player == humanPlayer)
                {
                    Debug.Log("Player won the race!");
                    battleManager.GameModeFinished(true);
                }
                else if(humanPlayer != null && player != humanPlayer)
                {
                    Debug.Log("Aww the player lost the race :(");
                    battleManager.GameModeFinished(false);
                }
                else
                {
                    Debug.Log("Player " + player.name + " won the race");
                    battleManager.GameModeFinished(true);
                }    
            }            
        }

        if(waypoint.start && lapCounter[player] == -1)
        {
            lapCounter[player] = 0;
        }        

        if(humanPlayer != null & player == humanPlayer)
        {
            audioSource.PlayOneShot(checkpointSound, 1f);
            waypoint.nextWaypoint.EnableVisuals();
            waypoint.DisableVisuals();
        }

        GetRacePositions();
        UpdatePlayerPositions();
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

    private void UpdatePlayerPositions()
    {
        int playerPos = activePlayers.IndexOf(humanPlayer);
        for (int i = 0; i < activePlayers.Count; i++)
        {
            activePlayers[i].GetComponent<CheckpointManager>().SetRacePosition(i, playerPos);
        }
        playerPos += 1;
        racePositionText.SetText(playerPos.ToString());
    }

    private List<Player> GetRacePositions()
    {
        activePlayers.Sort(SortByRacePosition);
        return activePlayers;
    }

    static int SortByRacePosition(Player a, Player b)
    {        
        if(lapCounter[a] > lapCounter[b])
        {
            return -1;
        }

        if(lapCounter[a] < lapCounter[b])
        {
            return 1;
        }

        if(checkpointCounter[a] > checkpointCounter[b])
        {
            return -1;
        }

        if(checkpointCounter[a] < checkpointCounter[b])
        {
            return 1;
        }

        float aDistanceToCheckpoint = Vector3.Distance(a.transform.position, a.GetComponent<CheckpointManager>().targetPoint.transform.position);
        float bDistanceToCheckpoint = Vector3.Distance(b.transform.position, b.GetComponent<CheckpointManager>().targetPoint.transform.position);

        if(aDistanceToCheckpoint < bDistanceToCheckpoint)
        {
            return -1;
        }

        if(aDistanceToCheckpoint > bDistanceToCheckpoint)
        {
            return 1;
        }
        
        //everything matched, somehow they are at the exact same point?
        Debug.LogWarning("Race: compareRacePositions returned the exact same when comparing: " + a.name + " and " + b.name);
        return 0; 
    }
}
