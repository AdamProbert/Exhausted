using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CampaignPlayerData
{

    public int playerCaps;
    public int playerScrap;
    public int playerFuel;

    public float[] position;

    public int carPrefabID;

    public CampaignPlayerData(CampaignPlayer player)
    {
        position = new float[3];

        playerCaps = player.caps;
        playerFuel = player.fuel;
        playerScrap = player.scrap;

        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
