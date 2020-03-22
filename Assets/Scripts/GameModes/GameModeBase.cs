using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class GameModeBase : MonoBehaviour
{
    [SerializeField] protected float timePerCinemachineMove = 3;

    public enum GameModeType
    {
        None,
        FreeForAll,
        Race
    }

    public GameModeType type;

    public abstract void Setup(int noOfEnemies, Player humanPlayer);
    public abstract void Setup(int noOfEnemies);

    public abstract void StartItUp(bool wholeSequence);
}
