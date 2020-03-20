using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerEventManager : MonoBehaviour
{

    // Actions
    public Action<GameManager.GameState> OnGameStateChanged;
    public Action<float> OnPlayerHealthChanged;
    public Action<float> OnPlayerArmourChanged;

    public Action<Player.state> OnPlayerStateChanged;

    public Action<Player> OnNavTargetChange;

    public Action<Player> OnWeaponTargetChange;

    public Action<Pickup> OnPickupItem;

    public Action<Player, Player.state> GlobalPlayerStateChange = delegate{};


    // Start is called before the first frame update
    void Awake()
    {
        // Listen for game state changes and propogate down
        GameManager.Instance.OnStateChange += HandleGameStateChange;
    }

    void HandleGameStateChange()
    {
        if(OnGameStateChanged != null)
        {
            OnGameStateChanged(GameManager.Instance.gameState);
        }
    }
}
