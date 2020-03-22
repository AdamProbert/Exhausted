using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] public GameModeBase.GameModeType spawnPointType;
    public Player occupied;
    [SerializeField] public CinemachineVirtualCamera virtualCamera; 
}
