using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private void Awake() 
    {
        
        GameManager.Instance.SetGameState(GameManager.GameState.MAIN_MENU);
    }

    // Button events
    public void OnClickLoadMap(string mapname)
    {
        GameManager.Instance.LoadMap(mapname);
    }
}
