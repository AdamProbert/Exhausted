using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class GameModeBase : MonoBehaviour
{
    public enum GameModeType
    {
        None,
        FreeForAll,
        Race
    }

    public GameModeType type;

    public abstract void Setup(int noOfEnemies, Player humanPlayer);
    public abstract void Setup(int noOfEnemies);
}
