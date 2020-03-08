using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public int playerCaps;
    public int playerScrap;
    public int playerFuel;

    public PlayerData(CampaignPlayer player)
    {   
        playerCaps = player.caps;
        playerFuel = player.fuel;
        playerScrap = player.scrap;
    }
}
