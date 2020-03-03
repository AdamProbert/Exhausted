using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<Player> enemyPrefabs;

    private List<SpawnPoint> FindSpawnPoints()
    {
        List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>();
        foreach(SpawnPoint p in Object.FindObjectsOfType<SpawnPoint>())
        {
            if(!p.occupied)
            {
                availableSpawnPoints.Add(p);
            }
        }
        return availableSpawnPoints;
    }

    public void SpawnThem(int noOfEnemiesToSpawn = -1)
    {
        List<SpawnPoint> availableSpawnPoints = FindSpawnPoints();
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

    public void SpawnPlayer(Player player)
    {
        List<SpawnPoint> availableSpawnPoints = FindSpawnPoints();
        int selection = Random.Range(0, availableSpawnPoints.Count);

        player.transform.position = availableSpawnPoints[selection].transform.position;
        player.transform.rotation = availableSpawnPoints[selection].transform.rotation;

        availableSpawnPoints[selection].occupied = player;
    }
}
