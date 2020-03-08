using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using UnityEngine;

public static class SaveSystem 
{
    static string playerSavePath = Application.persistentDataPath + "/player.save";
    static string playerConfigSavePath = Application.persistentDataPath + "/player_config.save";

    public static void SavePlayer(CampaignPlayer player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(playerSavePath, FileMode.Create);

        CampaignPlayerData data = new CampaignPlayerData(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static CampaignPlayerData LoadPlayer()
    {
        if(File.Exists(playerSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(playerSavePath, FileMode.Open);
            CampaignPlayerData data = formatter.Deserialize(stream) as CampaignPlayerData;
            stream.Close();
            return data;
        }   
        else
        {
            Debug.Log("Player save file not found in: " +playerSavePath);
            return null;
        }
    }

    public static void SavePlayerConfig(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(playerConfigSavePath, FileMode.Create);

        PlayerConfig data = new PlayerConfig(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerConfig LoadPlayerConfig()
    {
        if(File.Exists(playerConfigSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(playerConfigSavePath, FileMode.Open);
            try
            {
                PlayerConfig data = formatter.Deserialize(stream) as PlayerConfig;
                stream.Close();
                return data;
            }
            catch (SerializationException e)
            {
                Debug.Log("Serialization exception" + e);
                return null;
            }
            
            
        }   
        else
        {
            Debug.Log("Player config save file not found in: " +playerConfigSavePath);
            return null;
        }
    }
}
