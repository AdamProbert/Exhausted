using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
[RequireComponent(typeof(Spawner))]
public class BattleManager : MonoBehaviour
{
    private List<Player> activePlayers = new List<Player>();
    private List<Player> deadPlayers = new List<Player>();

    [SerializeField] Player humanPlayer;
    private IEnumerator endGameRoutine;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private bool autoEndGame = true;
    [SerializeField] public int requestedEnemyCount = 0;

    bool playerLoaded = false;

    // Start is called before the first frame update
    void Awake()
    {

        LoadPlayerVehicle();

        if(BattleData.noOfEnemies > 0)
        {
            requestedEnemyCount = BattleData.noOfEnemies;
        }

        // Spawn player in correct position first if they are here
        if(humanPlayer)
        {
            GetComponent<Spawner>().SpawnPlayer(humanPlayer);
        }
        
        // Then spawn the AI in the remaining places
        GetComponent<Spawner>().SpawnThem(requestedEnemyCount);

        // Then collect references to everyone
        foreach(Player p in FindObjectsOfType<Player>())
        {
            activePlayers.Add(p);
            p.OnStateChange += HandlePlayerStateChange;
        }

        endGameRoutine = EndGame();
        endGameUI.SetActive(false);
        
        // Lock cursor for gameplay
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start() 
    {
        GameManager.Instance.SetGameState(GameManager.GameState.PLAY);
    }

    public void HandlePlayerStateChange(Player.playerState newstate, Player player)
    {
        if(newstate == Player.playerState.Dead)
        {
            activePlayers.Remove(player);
            deadPlayers.Add(player);
            if(player == humanPlayer)
            {
                Debug.Log("Player died ending game");
                BattleData.winning = false;
                StartCoroutine(endGameRoutine);
            }   
        }

        if(activePlayers.Count == 1)
        {
            Debug.Log("One player left! " + activePlayers[0].name + " WON!");
            BattleData.winning = true;
            StartCoroutine(endGameRoutine);
        }
    }

    public void LoadPlayerVehicle()
    {
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
                        Attachment w = Instantiate(Resources.Load<GameObject>("CarWeapons/" + config.weaponPrefabNames[i])).GetComponent<Attachment>();

                        w.transform.position = attachPoints[i].transform.position;
                        w.transform.rotation = attachPoints[i].transform.rotation;
                        w.transform.parent = attachPoints[i].transform;
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

    private IEnumerator EndGame()
    {
        if(autoEndGame)
        {
            endGameUI.SetActive(true);
            yield return new WaitForSeconds(10);
            SceneManager.LoadScene("CampaignMap");
            Destroy(humanPlayer.gameObject); // Make sure we clean up the player
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
    }
}