using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<Player> enemyPrefabs;

    public List<SpawnPoint> GetAllSpawnPoints(GameModeBase.GameModeType type)
    {
        List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>();
        foreach(SpawnPoint p in Object.FindObjectsOfType<SpawnPoint>())
        {
            if(p.spawnPointType == type)
            {
                availableSpawnPoints.Add(p);
            }
        }
        return availableSpawnPoints;
    }
    
    public List<SpawnPoint> FindSpawnPoints(GameModeBase.GameModeType type)
    {
        List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>();
        foreach(SpawnPoint p in Object.FindObjectsOfType<SpawnPoint>())
        {
            if(!p.occupied & p.spawnPointType == type)
            {
                availableSpawnPoints.Add(p);
            }
        }
        return availableSpawnPoints;
    }

    public void SpawnThem(GameModeBase.GameModeType type, int noOfEnemiesToSpawn = -1)
    {
        List<SpawnPoint> availableSpawnPoints = FindSpawnPoints(type);
        if(noOfEnemiesToSpawn == -1)
            noOfEnemiesToSpawn = availableSpawnPoints.Count;

        if(noOfEnemiesToSpawn > availableSpawnPoints.Count)
        {
            noOfEnemiesToSpawn = availableSpawnPoints.Count;
        }

        for (int i = 0; i < noOfEnemiesToSpawn; i++)
        {
            // Instantiate new player, move to spawn point position
            Player p = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], availableSpawnPoints[i].transform.position, availableSpawnPoints[i].transform.rotation);
            availableSpawnPoints[i].occupied = p;
        }
    }

    public void SpawnPlayer(Player player, GameModeBase.GameModeType type)
    {
        List<SpawnPoint> availableSpawnPoints = FindSpawnPoints(type);
        int selection = Random.Range(0, availableSpawnPoints.Count);

        player.transform.position = availableSpawnPoints[selection].transform.position;
        player.transform.rotation = availableSpawnPoints[selection].transform.rotation;

        availableSpawnPoints[selection].occupied = player;
    }
}
