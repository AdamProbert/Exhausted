using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private void Awake() 
    {
        GameManager.Instance.SetGameState(GameManager.GameState.MAIN_MENU);
        // Unlock cursor for gameplay
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    // Button events
    public void OnClickLoadMap(string mapname)
    {
        GameManager.Instance.LoadMap(mapname);
    }

    public void LoadCampaign()
    {
        GameManager.Instance.LoadCampaign();
    }
}
