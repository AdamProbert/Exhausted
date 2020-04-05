
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
    private AsyncOperation LoadingAO;

    private void LoadScene(string sceneName)
    {
        IEnumerator lsr = LoadSceneRoutine(sceneName);
        StartCoroutine(lsr);
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        yield return new WaitForSeconds(3);
        LoadingAO = SceneManager.LoadSceneAsync(sceneName);
        LoadingAO.allowSceneActivation = false;
    }

    public void AllowNextSceneLoad()
    {
        LoadingAO.allowSceneActivation = true;
    }

    public float GetLoadingProgress()
    {   
        if(LoadingAO == null)
        {
            return 0;
        }
        return LoadingAO.progress;
    }

    public void LoadDessertScene()
    {
        this.LoadScene("DesertArena");
    }

    public void LoadCampaign()
    {
        this.LoadScene("CampaignMap");
    }

    public void LoadMap(string mapName)
    {
        requestedMap = mapName;
        this.LoadScene("BuildScene");
    }

    public void BuildComplete(Player player)
    {
        if(requestedMap != null)
        {
            this.LoadScene(requestedMap);    
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