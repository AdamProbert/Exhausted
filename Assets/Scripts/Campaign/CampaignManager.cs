using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignManager : MonoBehaviour
{

    protected CampaignManager() {}
    [SerializeField] public CampaignManager state { get; private set; }

    private static CampaignManager manager;
 
    public static CampaignManager instance
    {
        get
        {
            if (!manager)
            {
                manager = FindObjectOfType(typeof(CampaignManager)) as CampaignManager;
                if (!manager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
            }
            return manager;
        }
    }

    public void RequestBattle(int noOfEnemies, string mapScene)
    {
        CampaignEventManager.TriggerEvent("BattleStarting", null);
        BattleData.noOfEnemies = noOfEnemies;
        BattleData.returnScene = "CampaignMap";
        SceneManager.LoadScene(mapScene);
    }
}