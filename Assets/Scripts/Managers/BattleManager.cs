using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
 
[RequireComponent(typeof(Spawner))]
[RequireComponent(typeof(GameModeBase))]
public class BattleManager : MonoBehaviour
{
    PlayerEventManager playerEventManager;    
    [SerializeField] Player humanPlayerPrefab;
    Player humanPlayer;
    private IEnumerator endGameRoutine;
    private IEnumerator startGameRoutine;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private bool autoEndGame = true;
    [SerializeField] private bool skipStartSequence = true;
    [SerializeField] public int requestedEnemyCount = 0;
    [SerializeField] public GameModeBase.GameModeType requestedGameMode;

    [Header("Camera stuff")]
    [SerializeField] float timePerCinemachineMove = 3;

    Spawner spawner;
    GameModeBase gameModeBase;

    private void Awake() 
    {
        spawner = GetComponent<Spawner>();

        // Select the game mode
        foreach (GameModeBase gmb in GetComponents<GameModeBase>())
        {
            if(gmb.type == requestedGameMode)
            {
                gameModeBase = gmb;        
            }
        }
        
        if(humanPlayerPrefab != null)
        {
            LoadPlayerVehicle();
        }

        if(requestedEnemyCount == -1)
        {
            requestedEnemyCount = 0;
        }
        else if(BattleData.noOfEnemies > 0)
        {
            requestedEnemyCount = BattleData.noOfEnemies;
        }

        // Spawn player in correct position first if they are here
        if(humanPlayer)
        {
            spawner.SpawnPlayer(humanPlayer);
            playerEventManager = humanPlayer.GetComponent<PlayerEventManager>();
        }
        
        // Then spawn the AI in the remaining places
        spawner.SpawnThem(requestedEnemyCount);
    }

    void Start()
    {
        if(humanPlayer != null)
        {
            gameModeBase.Setup(requestedEnemyCount, humanPlayer);
        }
        else
        {
            gameModeBase.Setup(requestedEnemyCount);
        }
        

        startGameRoutine = StartGame();
        endGameRoutine = EndGame();
        endGameUI.SetActive(false);
        
        // Lock cursor for gameplay
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        if(!skipStartSequence)
        {
            StartCoroutine(startGameRoutine);
        }
        else
        {
            GameManager.Instance.SetGameState(GameManager.GameState.PLAY);
        }
    }

    public void GameModeFinished(bool playerWon)
    {
        BattleData.winning = playerWon;
        StartCoroutine(endGameRoutine);
    }

    public void LoadPlayerVehicle()
    {
        humanPlayer = Instantiate(humanPlayerPrefab);
        PlayerConfig config = SaveSystem.LoadPlayerConfig();
        if(config.baseCarPrefabName != null)
        {
            GameObject vehicle = Instantiate(Resources.Load("Cars/" + config.baseCarPrefabName) as GameObject, humanPlayer.transform.position, humanPlayer.transform.rotation, humanPlayer.transform);
            CarAttachPoint[] attachPoints = vehicle.GetComponentsInChildren<CarAttachPoint>();

            if(config.weaponPrefabNames != null)
            {
                for (int i = 0; i < config.weaponPrefabNames.Length; i++)
                {
                    if(config.weaponPrefabNames[i] != "NONE")
                    {
                        Attachment w = Instantiate(
                            Resources.Load<GameObject>(
                                "CarWeapons/" + config.weaponPrefabNames[i]),
                                attachPoints[i].transform.position,
                                attachPoints[i].transform.rotation,
                                attachPoints[i].transform
                        ).GetComponent<Attachment>();
                        
                        attachPoints[i].Attach(w);    
                    }
                }
            }
        }
        // If no saved vehicle - load the default golf cart lol
        else
        {
            Debug.Log("No saved vehicle! - go make one");
        }

    }

    private IEnumerator StartGame() 
    {
        CinemachineFreeLook playerCam = humanPlayer.GetComponentInChildren<CinemachineFreeLook>();
        List<SpawnPoint> spawnPoints = spawner.GetAllSpawnPoints();
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

        GameManager.Instance.SetGameState(GameManager.GameState.PLAY);
    }

    private IEnumerator EndGame()
    {
        if(autoEndGame)
        {
            endGameUI.SetActive(true);
            yield return new WaitForSeconds(5);
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            
            if(BattleData.returnScene != null)
            {
                SceneManager.LoadScene(BattleData.returnScene);
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }   
        }
    }
}