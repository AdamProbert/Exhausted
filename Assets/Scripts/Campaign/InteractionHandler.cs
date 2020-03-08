using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [Header("Trade")]
    [SerializeField] int fuelCost;
    [SerializeField] int scrapCost;

    [Header("Battle")]
    [SerializeField] int enemyCount;

    CampaignPlayer player;

    private void Start() 
    {
        player = GameObject.FindObjectOfType<CampaignPlayer>();
    }

    public void OnClickTrade(string type)
    {
        switch (type)
        {
            case "FUEL":
                if(player.caps > fuelCost)
                {
                    player.DecreaseCaps(fuelCost);
                    player.IncreaseFuel(1);
                }
                break;
            
            default:
                break;
        }
    }

    public void OnClickBattle()
    {
        CampaignManager.instance.RequestBattle(enemyCount);
    }
 
    public void DoAThing(int bleh)
    {

    }
}
