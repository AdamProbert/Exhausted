
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    // Game States
    // for now we are only using these two
    public enum GameState { INTRO, MAIN_MENU, BUILD, PLAY }

    public delegate void OnStateChangeHandler();

    // Game manager shiz

    protected GameManager() {}
    public event OnStateChangeHandler OnStateChange;
    [SerializeField] public GameState gameState { get; private set; }

    // Map selection shiz
    [SerializeField] private string requestedMap;

    
    public void LoadDessertScene()
    {
        SceneManager.LoadScene("DesertArena");
    }

    public void LoadCampaign()
    {
        SceneManager.LoadScene("CampaignMap");
    }

    public void LoadMap(string mapName)
    {
        requestedMap = mapName;
        SceneManager.LoadScene("BuildScene");
    }

    public void BuildComplete(Player player)
    {
        DontDestroyOnLoad(player);
        if(requestedMap != null)
        {
            SceneManager.LoadScene(requestedMap);    
        }
        else
        {
            LoadDessertScene();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetGameState(GameState state){
        Debug.Log("GameManager set game state called. New state: " + state);
        this.gameState = state;
        if(OnStateChange != null)
        {
            OnStateChange();
        }
        
    }
}