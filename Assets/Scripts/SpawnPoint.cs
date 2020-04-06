using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] public GameModeBase.GameModeType spawnPointType;
    public Player occupied;
    [SerializeField] public CinemachineVirtualCamera virtualCamera;

    private void Awake() 
    {
        Vector3 newPosition = virtualCamera.transform.position;
        newPosition.y += Random.Range(20,30);
        newPosition.x += Random.Range(-20,20);
        newPosition.z += Random.Range(-20,20);
        virtualCamera.transform.position = newPosition;
    }
}
