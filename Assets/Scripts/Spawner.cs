using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<Player> enemyPrefabs;
    private List<GameObject> availableSpawnPoints = new List<GameObject>();

    private void FindSpawnPoints()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            availableSpawnPoints.Add(g);
        }
    }

    public void SpawnThem(int noOfEnemiesToSpawn = -1)
    {
        FindSpawnPoints();
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
}
