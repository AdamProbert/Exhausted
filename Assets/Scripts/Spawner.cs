using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<Player> enemyPrefabs;

    private List<GameObject> FindSpawnPoints()
    {
        List<GameObject> availableSpawnPoints = new List<GameObject>();
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            availableSpawnPoints.Add(g);
        }
        return availableSpawnPoints;
    }

    public void SpawnThem(int noOfEnemiesToSpawn = -1)
    {
        List<GameObject> availableSpawnPoints = FindSpawnPoints();
        if(noOfEnemiesToSpawn == -1)
            noOfEnemiesToSpawn = availableSpawnPoints.Count;

        if(noOfEnemiesToSpawn > availableSpawnPoints.Count)
        {
            noOfEnemiesToSpawn = availableSpawnPoints.Count;
        }

        for (int i = 0; i < noOfEnemiesToSpawn; i++)
        {
            // Instantiate new player, move to spawn point position
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], availableSpawnPoints[i].transform.position, availableSpawnPoints[i].transform.rotation);
        }
    }

    public void SpawnPlayer(Player player)
    {
        List<GameObject> availableSpawnPoints = FindSpawnPoints();
        int selection = Random.Range(0, availableSpawnPoints.Count);

        player.transform.position = availableSpawnPoints[selection].transform.position;
        player.transform.rotation = availableSpawnPoints[selection].transform.rotation;
        Destroy(availableSpawnPoints[selection].gameObject);
    }
}
