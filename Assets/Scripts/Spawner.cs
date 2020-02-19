using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<Player> enemyPrefabs;
    private List<GameObject> availableSpawnPoints = new List<GameObject>();

    private void Start() 
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            availableSpawnPoints.Add(g);
        }

    }

    public void SpawnThem(int noOfEnemiesToSpawn = -1)
    {
        if(noOfEnemiesToSpawn == -1)
            noOfEnemiesToSpawn = availableSpawnPoints.Count;

        for (int i = 0; i < noOfEnemiesToSpawn; i++)
        {
            // Instantiate new player, move to spawn point position
        }

        // Make all spawn points invisible - either through layer and camera layermask or by changing the renderer
    }

    private void Spawn(Player enemyToSpawn)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
